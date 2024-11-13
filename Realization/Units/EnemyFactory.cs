using System.Collections.Generic;
using System.Linq;
using Battle;
using DefaultNamespace;
using Fight;
using Fight.Fractions;
using Grids;
using Infrastructure.Services.StaticData;
using Infrastructure.Shared.Extensions;
using Model.Economy;
using Parameters;
using Realization.States.CharacterSheet;
using Realization.TutorialRealization.Helpers;
using Realization.UnitplaceSets;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.General
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private bool _disable = false;

        private CharacterConfig _characterConfig;
        private MinionFactory _factory;
        private IGrid<IMinion> _grid;
        private IEnemiesPool _enemiesPool;
        private List<IMinion> _minions = new List<IMinion>();
        private IStaticDataService _staticData;
        private IStorage _storage;

        [Inject]
        private void Construct(MinionFactory factory, IGrid<IMinion> grid, CharacterConfig characterConfig,
                IEnemiesPool enemiesPool, IStaticDataService staticDataService,IStorage storage)
        {
            _storage = storage;
            _staticData = staticDataService;
            _enemiesPool = enemiesPool;
            _factory = factory;
            _grid = grid;
            _characterConfig = characterConfig;
        }

        public IMinion[] Create(int number)
        {
            if (_disable)
                return null;

            UnbindAlEnemies();

            var levelIndex = PlayerPrefs.GetInt("level");

            if (levelIndex > 0)
                levelIndex--;

            //var dungeon = _staticData.ForDungeons(Constants.DungeonKey + _storage.PlayerProgress.Bioms.CurrentDungeon);
            
            BiomeData biomeData = _staticData.ForBioms(_storage.PlayerProgress.Bioms.SelectedBiom.Key);
            var dungeon = biomeData.ForStage(_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber);
            
            var roomCurrent = dungeon._rooms[number];

            string[] tags = { "enemy", dungeon.FractionEnemyTags };
            _minions = new List<IMinion>(CreateEnemies(ref roomCurrent, tags, "boss"));

            var autoPlace = new AutoPlace(_grid, true);
            autoPlace.PlaceMinions(_minions.ToArray());
            foreach (var minion in _minions)
            {
                minion.UpdateWorldPosition(MoveType.Instantly);
            }
            return _minions.ToArray();
        }

        public IMinion Create(Character character)
        {
            IMinion minion = _factory.CreateAndReturn(character);
            _enemiesPool.TryAdd(minion);
            if (HardTutorial.Activated)
            {
                HardTutorial.Instance.Include(minion.GameObject);
            }

            return minion;
        }

        private IMinion[] CreateEnemies(ref Room room, string[] tags, string exceptTag = "")
        {
            List<IMinion> minions = new();

            for (int i = 0; i < room.Lvl_1; i++)
            {
                Character character = CreateNewCharacter(tags, exceptTag, 1);
                IMinion minion = _factory.CreateAndReturn(character);
                _enemiesPool.TryAdd(minion);
                LoadChild(minion, i);
                minions.Add(minion);
                if (HardTutorial.Activated)
                {
                    HardTutorial.Instance.Include(minion.GameObject);
                }
            }
            for (int i = 0; i < room.Lvl_2; i++)
            {
                Character character = CreateNewCharacter(tags, exceptTag, 2);
                IMinion minion = _factory.CreateAndReturn(character);
                _enemiesPool.TryAdd(minion);
                LoadChild(minion, i);
                minions.Add(minion);
                if (HardTutorial.Activated)
                {
                    HardTutorial.Instance.Include(minion.GameObject);
                }
            }
            for (int i = 0; i < room.Lvl_3; i++)
            {
                Character character = CreateNewCharacter(tags, exceptTag, 3);
                IMinion minion = _factory.CreateAndReturn(character);
                _enemiesPool.TryAdd(minion);
                LoadChild(minion, i);
                minions.Add(minion);
                if (HardTutorial.Activated)
                {
                    HardTutorial.Instance.Include(minion.GameObject);
                }
            }
            for (int i = 0; i < room.Lvl_4; i++)
            {
                Character character = CreateNewCharacter(tags, exceptTag, 4);
                IMinion minion = _factory.CreateAndReturn(character);
                _enemiesPool.TryAdd(minion);
                LoadChild(minion, i);
                minions.Add(minion);
                if (HardTutorial.Activated)
                {
                    HardTutorial.Instance.Include(minion.GameObject);
                }
            }
            for (int i = 0; i < room.Lvl_5; i++)
            {
                Character character = CreateNewCharacter(tags, exceptTag, 5);
                IMinion minion = _factory.CreateAndReturn(character);
                _enemiesPool.TryAdd(minion);
                LoadChild(minion, i);
                minions.Add(minion);
                if (HardTutorial.Activated)
                {
                    HardTutorial.Instance.Include(minion.GameObject);
                }
            }

            return minions.ToArray();
        }

        public IMinion CreateWithBoss()
        {
            // var dungeon = _staticData.ForDungeons(Constants.DungeonKey + _storage.PlayerProgress.Bioms.CurrentDungeon);
            UnbindAlEnemies();

            BiomeData biomeData = _staticData.ForBioms(_storage.PlayerProgress.Bioms.SelectedBiom.Key);
            var dungeon = biomeData.ForStage(_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber);

            var room = new Room();
            room.Lvl_1 = 1;
            var boss = CreateEnemies(ref room, new[] {"boss", dungeon.FractionEnemyTags});
            boss[0].IncreaseView(1.5f);

            RemoveReward(boss[0]);

            var autoPlace = new AutoPlace(_grid, true);
            autoPlace.PlaceMinions(boss[0]);
            boss[0].UpdateWorldPosition(MoveType.Instantly);
            return boss[0];
        }

        private void UnbindAlEnemies()
        {
            foreach (var obj in _grid.Objects.ToArray())
            {
                if (obj.Value != null && obj.Value.Fraction == Fraction.Enemies)
                {
                    _grid.Unbind(obj.Value);
                }
            }
        }

        private void RemoveReward(IMinion boss)
        {
            DeathReward deathReward = boss.GameObject.GetComponent<DeathReward>();
            Destroy(deathReward);
            ControlBars controlBars = boss.GameObject.GetComponentInChildren<ControlBars>();
            controlBars.EnableEnergyBar();
        }


        private void LoadChild(IMinion minion, int index)
        {
            // int positionX = _grid.Width - 1;
            // int positionY = _grid.Height - index - 1;
            //
            // _grid.Place(minion, positionX, positionY);
        }

        private Character CreateNewCharacter(string[] tags, string except = "", int startLevel = 1)
        {
            Character item;
            Character character;

            PriorityConfig priorityConfig = _characterConfig.Priorities[0];

            if (tags == null || tags.Length == 0)
            {
                item = _characterConfig.Characters.Random();
            }
            else if (except == "")
            {
                IEnumerable<Character> characters = new List<Character>(_characterConfig.Characters);
                foreach (var tag in tags)
                {
                    characters = characters.Where((character1 =>
                            character1.Tags != null && character1.Tags.Contains(tag)));
                }

                item = characters.Random();
            }
            else
            {
                IEnumerable<Character> characters = new List<Character>(_characterConfig.Characters);
                foreach (var tag in tags)
                {
                    characters = characters.Where((character1 =>
                            character1.Tags != null && character1.Tags.Contains(tag)));
                }

                item = characters
                        .Where((character1 => character1.Tags != null && character1.Tags.Contains(except) == false))
                        .Random();
            }

            if (item != null)
            {
                switch (item.Class)
                {
                    case MinionClass.Gladiator:
                        priorityConfig = _characterConfig.Priorities[0];
                        break;
                    case MinionClass.Templar:
                        priorityConfig = _characterConfig.Priorities[1];
                        break;
                    case MinionClass.Ranger:
                        priorityConfig = _characterConfig.Priorities[2];
                        break;
                    case MinionClass.Assassin:
                        priorityConfig = _characterConfig.Priorities[3];
                        break;
                    case MinionClass.Spiritmaster:
                        priorityConfig = _characterConfig.Priorities[4];
                        break;
                    case MinionClass.Cleric:
                        priorityConfig = _characterConfig.Priorities[5];
                        break;
                    case MinionClass.Chanter:
                        priorityConfig = _characterConfig.Priorities[6];
                        break;
                }

                item.PriorityConfig = priorityConfig;
            }

            item.Level = startLevel;

            character = new Character()
            {
                Uid = item.Uid,
                Class = item.Class,
                Prefab = item.Prefab,
                Grade = item.Grade,
                Tags = item.Tags,
                Health = item.Health,
                Armor = item.Armor,
                Power = item.Power,
                TimeBetweenAttacks = item.TimeBetweenAttacks,
                Range = item.Range,
                CriticalDamageChance = item.CriticalDamageChance,
                CriticalDamageMultiplier = item.CriticalDamageMultiplier,
                ChanceOfDodge = item.ChanceOfDodge,
                Energy = item.Energy,
                PowerOfHealing = item.PowerOfHealing,
                AdditionalParameter = item.AdditionalParameter,
                Level = item.Level,
                Skill = item.Skill,
                PriorityConfig = item.PriorityConfig
            };
            return character;
        }
    }
}