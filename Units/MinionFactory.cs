using System;
using System.Collections.Generic;
using System.Linq;
using CompositeDirectorWithGeneratingComposites.CompositeDirector;
using CustomInput.Picking;
using Fight.Fractions;
using Grids;
using Infrastructure.Helpers;
using Infrastructure.Services.CharacteristicSetupService;
using Model.Commands;
using Model.Commands.Parts;
using Model.Economy;
using Model.Maps;
using Parameters;
using Plugins.CompositeDirectorPlugin;
using Realization.Configs;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using Realization.States.CharacterSheet;
using Realization.TutorialRealization.Helpers;
using Realization.UnitplaceSets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using CharacterStore = Parameters.CharacterStore;

namespace Units
{
    public class MinionFactory : Infrastructure.Shared.Factories.IFactory<Character>, IMinionPool
    {
        private const Currency currency = Currency.Gold;
        private readonly DiContainer _container;

        private readonly Dictionary<MinionClass, IMinion> _playerMinions;
        private readonly Dictionary<MinionClass, IMinion> _enemyMinions;
        private readonly IMap _map;
        private readonly Vector2 _roomSize;
        private Transform _parent;
        private int _allyIndex;
        private int _enemyIndex;
        private Constants _constants;
        private readonly CharacterStore[] _storeCharacters;
        private readonly List<IMinion> _minions = new();

        public List<IMinion> Minions => _minions;
        public static List<IMinion> Units = new();
        private IGrid<IMinion> _grid;
        public event Action<IMinion> Created;

        private PriorityConfig[] _priorityConfig;
        private int[] _priorites;
        private CompositeDirector _director;
        private CommandFacade _commandFacade;
        private Skill[] _skills;
        private readonly ICharacteristicSetupService _characteristicSetupService;

        public MinionFactory(DiContainer container, Dictionary<MinionClass, IMinion> playerMinions,
                Dictionary<MinionClass, IMinion> enemyMinions, IMap map,
                MapConfig mapConfig, Transform parent, Constants constants, CharacterStore[] storeCharacters,
                IGrid<IMinion> grid, PriorityConfig[] priorityConfig,
                CompositeDirector director,
                CommandFacade commandFacade, Skill[] skills,ICharacteristicSetupService characteristicSetupService)
        {
            _skills = skills;
            _characteristicSetupService = characteristicSetupService;
            _commandFacade = commandFacade;
            _director = director;
            _grid = grid;
            _constants = constants;
            _storeCharacters = storeCharacters;
            _parent = parent;
            _playerMinions = playerMinions;
            _enemyMinions = enemyMinions;
            _container = container;
            _map = map;
            _roomSize = mapConfig.RoomSize;
            _priorityConfig = priorityConfig;
            Units.Clear();
        }

        public void Create(Character character)
        {
            CreateAndPlace(character);
        }

        public IMinion CreateAndPlace(Character character, bool onAllGrid = false)
        {
            var minion = CreateAndReturn(character);
            var placer = new AutoPlace(_grid, false);
            if (placer.PlaceMinions(minion) == false && onAllGrid)
            {
                placer = new AutoPlace(_grid, true);
                placer.PlaceMinions(minion);
            }
            return minion;
        }

        public IMinion CreateAndReturn(Character minionClass)
        {
            PriorityConfig priorityConfig = _priorityConfig[0];
            Vector2 position = _map.Current.Position * _roomSize;

            var fraction = minionClass.Tags.Contains("ally") ? Fraction.Minions : Fraction.Enemies;
            GameObject classPrefab = fraction == Fraction.Minions
                    ? _playerMinions[minionClass.Class].GameObject
                    : _enemyMinions[minionClass.Class].GameObject;

            GameObject minionObject = _container.InstantiatePrefab(
                    classPrefab,
                    position,
                    Quaternion.identity,
                    null);
            minionObject.transform.SetParent(_parent);
            if (fraction == Fraction.Minions)
            {
                minionObject.name = $"Ally_{_allyIndex}";
                _allyIndex++;
            }
            else
            {
                minionObject.name = $"Enemy_{_enemyIndex}";
                _enemyIndex++;
            }
            minionObject.transform.localScale = Vector3.one;
            IMinion minion = minionObject.GetComponent<IMinion>();

            var pair = CreateSellingPair(minionClass.Grade, _storeCharacters);

            priorityConfig = FindListPriorities(minionClass);

            var skills = _skills.Where((skill => skill.Uid == minionClass.Skill)).ToArray();
            minion.Initialize(minionClass, pair, _constants, _grid, priorityConfig, _commandFacade, skills);
            if (fraction == Fraction.Minions)
            {
                _characteristicSetupService.SetupCharacteristic(minion.Parameters, minion.ParentId);
                _characteristicSetupService.SetupGeneralCharacteristic(minion.Parameters);
                Pickable pickable = minionObject.GetComponentInChildren<Pickable>();
                pickable.RegisterInfoHint(minion.Class,minion.Grade);
            }
            Minions.Add(minion);
            if (minion.Fraction == Fraction.Minions)
            {
                MinionFactory.Units.Add(minion);

                for (int i = 0; i < Units.Count; i++)
                {
                    Units[i].UpdateListMinion(Units.ToArray());
                }
            }

            if (HardTutorial.Activated)
            {
                HardTutorial.Instance.DisableMinion(minionObject);
            }

            minion.Died += RemoveMinion;
            if (fraction == Fraction.Minions)
            {
                minion.SellingHandler.Sell += RemoveMinion;
            }
            _director.Add(minion);
            Created?.Invoke(minion);
            SceneObjectPool
                .AddRange(minion.GameObject.GetComponentsInChildren<Transform>()
                    .Select((transform => transform.gameObject)).ToArray());
            return minion;
        }

        private PriorityConfig FindListPriorities(Character item)
        {
            PriorityConfig priorityConfig = _priorityConfig[0];

            switch (item.Class)
            {
                case MinionClass.Gladiator:
                    priorityConfig = _priorityConfig[0];
                    break;
                case MinionClass.Templar:
                    priorityConfig = _priorityConfig[1];
                    break;
                case MinionClass.Ranger:
                    priorityConfig = _priorityConfig[2];
                    break;
                case MinionClass.Assassin:
                    priorityConfig = _priorityConfig[3];
                    break;
                case MinionClass.Spiritmaster:
                    priorityConfig = _priorityConfig[4];
                    break;
                case MinionClass.Sorcerer:
                    priorityConfig = _priorityConfig[5];
                    break;
                case MinionClass.Cleric:
                    priorityConfig = _priorityConfig[6];
                    break;
                case MinionClass.Chanter:
                    priorityConfig = _priorityConfig[7];
                    break;
            }

            return priorityConfig;
        }

        private void RemoveMinion(IMinion obj)
        {

            _minions.Remove(obj);
            _director.Remove(obj);
            
            for(int i = 0; i < Units.Count; i++)
            {
                Units[i].DiedMinion(obj);
            }
            
            if (obj.Fraction == Fraction.Minions)
            {
                MinionFactory.Units.Remove(obj);

                if (MinionFactory.Units.Count == 0 && _minions.Count != 0)
                {
                    PlayerPrefs.SetInt("level", 1);

                    if(SceneManager.GetActiveScene().name == "FightTest")
                        return;
                }
            }
        }

        private CurrencyValuePair CreateSellingPair(int grade, CharacterStore[] charactersStore)
        {
            int[] sellCost = new int[5] { 100, 100, 100, 100, 100 };

            var characterStoreConfig = charactersStore.FirstOrDefault((setup => setup.Grade == grade));
            if (characterStoreConfig != null)
            {
                sellCost[0] = characterStoreConfig.Sell_1;
                sellCost[1] = characterStoreConfig.Sell_2;
                sellCost[2] = characterStoreConfig.Sell_3;
                sellCost[3] = characterStoreConfig.Sell_4;
                sellCost[4] = characterStoreConfig.Sell_5;
            }

            return new CurrencyValuePair(sellCost, currency);
        }

        public bool CanCreate()
        {
            var placer = new AutoPlace(_grid, false);
            var havePlaces = placer.HasPlaces();
            return havePlaces;
        }
    }
}