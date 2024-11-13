using System;
using System.Collections.Generic;
using System.Linq;
using Entities.Tile;
using Entities.Tile.Views;
using Infrastructure.CompositeDirector;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Model.Maps;
using Model.Maps.Generators;
using Model.Maps.Types;
using Parameters;
using Realization.Configs;
using Realization.Economy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Constants = DefaultNamespace.Constants;

namespace Realization.Installers
{
    // TODO: separate
    public class MapInstaller : MonoInstaller
    {
        [SerializeField] private MapConfig[] _configs;
        [SerializeField] private Transform _parent;
        [SerializeField] private bool _tutorial = true;
        [SerializeField] private RoomCounter roomCounter;

        private CompositeDirector _director;
        private UnitCounter _counter;
        private IStaticDataService _staticData;
        private IStorage _storage;

        [Inject]
        private void Construct(CompositeDirector director, IStorage storage,IStaticDataService staticDataService)
        {
            _director = director;
            _staticData = staticDataService;
            _storage = storage;
        }

        public override void InstallBindings()
        {
            ConfigBiom configBiom = null;
            if (_storage.PlayerProgress != null) //TODO only for TestFight scene should be redone
            {
                /*int dungeon = _storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber;
                dungeonsConfig = _staticData.ForDungeons(Constants.DungeonKey + dungeon);*/
                BiomeData biomeData = _staticData.ForBioms(_storage.PlayerProgress.Bioms.SelectedBiom.Key);
                configBiom = biomeData.ForStage(_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber);
            }
            

            int index = PlayerPrefs.GetInt("level");
            MapConfig config = _configs[0];

            PlayableTileView[] shops = config.ShopTileViewPrefabs;
            int level = PlayerPrefs.GetInt("level");
            if (level == 0 && _tutorial)
                shops = shops.Reverse().ToArray();
            TileViewFactory factory = new TileViewFactory(Container, new Dictionary<Type, PlayableTileView[]>()
            {
                {typeof(StartTile), new[] {config.StartTileViewPrefab}},
                {typeof(EmptyTile), new[] {config.EmptyTileViewPrefab}},
                {typeof(ShopTile), shops},
                {typeof(BossTile), new[] {config.BossTileViewPrefab}},
            }, _parent);

            int roomcount = SceneManager.GetActiveScene().name == Constants.FightTestScene ? config.MapSize : configBiom._rooms.Count;
            if (roomcount > 0) roomcount-=2;
            IMapGenerator generator =
                new LineMapGenerator(
                        roomcount,
                    config.MinSegmentLength,
                    config.ShopCount,
                    config.SpawnShopsOnEmpty,
                    config.RotateChance,
                    config.ChangeRoomChance);
            /*if (index == 0 && _tutorial)
                generator = new TutorialGenerator();*/
            IMap map = generator.Generate();
            _counter = new UnitCounter();

            TileFactory tileFactory = new TileFactory(_director, factory, map);

            Container.Bind<TileFactory>().FromInstance(tileFactory).AsSingle();
            Container.Bind<MapConfig>().FromInstance(config).AsSingle();
            Container.Bind<IMap>().FromInstance(map).AsSingle();
            Container.Bind<UnitCounter>().FromInstance(_counter).AsSingle();

            Container.BindInterfacesTo<MapInstaller>().FromInstance(this).AsSingle();
            
            Container.Inject(roomCounter);
        }
    }
}