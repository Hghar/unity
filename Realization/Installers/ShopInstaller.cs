using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Entities.ShopItems;
using Entities.ShopItems.Views;
using Fight.Fractions;
using Grids;
using Grids.Helpers;
using Infrastructure.CompositeDirector;
using Infrastructure.Services.CharacteristicSetupService;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.WindowService;
using Model.Commands;
using Model.Commands.Creation;
using Model.Commands.Parts;
using Model.Economy;
using Model.Maps;
using Realization.Configs;
using Realization.NewMovers;
using Realization.Shops;
using Realization.States;
using Realization.States.CharacterSheet;
using Ticking;
using Units;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using NewDirector = Plugins.CompositeDirectorPlugin.CompositeDirector;

namespace Realization.Installers
{
    public class ShopInstaller : MonoInstaller
    {
        [SerializeField] private ShopItemView _prefab;
        [SerializeField] private Transform _unitParent;
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private Transform _shopParent;
        [SerializeField] private StartBattleButton _startBattleButton;
        [SerializeField] private ShopBehaviour _shopBehaviourPrefab;
        [SerializeField] private List<FractionPair> FractionUnits = new List<FractionPair>();
        [SerializeField] private InfoHint _infoHint;

        private CompositeDirector _director;
        private IMap _map;
        private MapConfig _mapConfig;
        private IStorage _storage;
        private IGrid<IMinion> _grid;
        private DragGridPlacer _dragger;
        private MinionPositionUpdater _updater;
        private ITickablePool _tickablePool;
        private GridMover _gridMover;
        private IBattleFinishPublisher _battleFinishPublisher;
        private IBattleStartPublisher _battleStartPublisher;
        private NewDirector _newDirector;
        private CommandFacade _commandFacade;
        private IStaticDataService _staticData;

        public static Shop Shop;
        public static MinionFactory MinionFactory;

        [Inject]
        private void Construct(CompositeDirector director, IMap map, MapConfig mapConfig, IStorage storage, 
            IGrid<IMinion> grid, ITickablePool tickablePool,IBattleStartPublisher battleStartPublisher, 
            IBattleFinishPublisher battleFinishPublisher, NewDirector newDirector,IStaticDataService staticDataService)
        {
            _staticData = staticDataService;
            _newDirector = newDirector;
            _battleStartPublisher = battleStartPublisher;
            _battleFinishPublisher = battleFinishPublisher;
            _tickablePool = tickablePool;
            _grid = grid;
            _storage = storage;
            _mapConfig = mapConfig;
            _map = map;
            _director = director;
        }

        public override void InstallBindings()
        {
            var enemySpawner = FindObjectOfType<EnemySpawner>();
            var minionsUnits = FractionUnits.First(x => x.Fraction == Fraction.Minions).UnitDictionary.ToDictionary();
            var enemyUnits = FractionUnits.First(x => x.Fraction == Fraction.Enemies).UnitDictionary.ToDictionary();
            MinionFactory minionFactory =
                    new MinionFactory(Container, minionsUnits,
                            enemyUnits, _map, _mapConfig, _unitParent,
                            _characterConfig.Constants, _characterConfig.CharacterStore, _grid,
                            _characterConfig.Priorities,
                            _newDirector, Container.Resolve<CommandFacade>(), 
                            _characterConfig.Skills.ToArray(),Container.Resolve<ICharacteristicSetupService>());
            
            MinionFactory = minionFactory;
            _dragger = new DragGridPlacer(_grid, MinionFactory);

            var icons = Resources.Load<Icons>("Icons");

            ShopItemEntityFactory shopItemEntityFactory = new ShopItemEntityFactory(_director, _prefab,
                    _characterConfig, _storage, MinionFactory, icons.Get(), _battleStartPublisher,
                    _battleFinishPublisher, _infoHint,_staticData);

            ShopBehaviour behaviour = Instantiate(_shopBehaviourPrefab, _shopParent);
            var shop = new Shop(
                    behaviour, shopItemEntityFactory, _characterConfig.Constants, _storage,
                    _characterConfig.CharacterStore,
                    _characterConfig.StoreСharactersСhances.ToArray(),
                    _startBattleButton,
                    _staticData,
                    enemySpawner,
                    minionFactory,
                    Container.Resolve<ICoroutineRunner>());
            Shop = shop;
            Container.Inject(behaviour.gameObject.GetComponentInChildren<OpenWindowButton>());
            Container.BindInterfacesAndSelfTo<Shop>().FromInstance(shop).AsSingle();
            Container.Bind<MinionFactory>().FromInstance(MinionFactory).AsSingle();
            Container.Bind<IMinionPool>().FromInstance(MinionFactory).AsSingle();
            Container.Bind<ShopItemEntityFactory>().FromInstance(shopItemEntityFactory).AsSingle();
            
            //todo remake
            Container.Resolve<CommandBuilder>().Initialize(MinionFactory, MinionFactory, _characterConfig.Characters);
        }

        private void Awake()
        {
            _updater = new MinionPositionUpdater(MinionFactory, _map, _battleFinishPublisher, _grid, _battleStartPublisher);
            _gridMover = new GridMover(_tickablePool, _grid, _characterConfig.Constants);
        }

        private void OnDestroy()
        {
            _dragger.Dispose();
            _updater.Dispose();
        }
    }

    [Serializable]
    public struct UnitDictionary
    {
        [SerializeField] private UnitPair[] _pairs;

        public Dictionary<MinionClass, IMinion> ToDictionary()
        {
            Dictionary<MinionClass, IMinion> units = new Dictionary<MinionClass, IMinion>();
            foreach (UnitPair pair in _pairs)
            {
                units.Add(pair.Class, (pair.Minion as GameObject).GetComponent<IMinion>());
            }

            return units;
        }
    }

    [Serializable]
    public struct UnitPair
    {
        public MinionClass Class;
        [SerializeReference] public Object Minion;
    }

    [Serializable]
    public class FractionPair
    {
        public Fraction Fraction;
        public UnitDictionary UnitDictionary;
    }
}