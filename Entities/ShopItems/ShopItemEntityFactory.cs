using System.Collections.Generic;
using Battle;
using Entities.Factories;
using Entities.ShopItems.Views;
using Infrastructure.CompositeDirector;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Model.Shops;
using Realization.Configs;
using Realization.Shops;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;

namespace Entities.ShopItems
{
    public class ShopItemEntityFactory : EntityFactory<ShopItemEntity, ShopItemEntityFactoryArgs>
    {
        private readonly ShopItemView _defaultItemPrefab;
        private readonly List<ShopItemEntity> _entities = new();
        private int _index;
        private IStorage _storage;
        private CharacterConfig _config;
        private MinionFactory _factory;
        private IconsProvider _iconsProvider;
        private IBattleStartPublisher _startPublisher;
        private IBattleFinishPublisher _finishPublisher;
        private InfoHint _infoHint;
        private readonly IStaticDataService _staticDataService;

        public ShopItemEntity[] Entities => _entities.ToArray();

        public ShopItemEntityFactory(CompositeDirector director, ShopItemView defaultItemPrefab,
            CharacterConfig config, IStorage storage, MinionFactory factory, IconsProvider iconsProvider,
            IBattleStartPublisher startPublisher, IBattleFinishPublisher finishPublisher, InfoHint infoHint,IStaticDataService staticDataService) : base(director)
        {
            _finishPublisher = finishPublisher;
            _startPublisher = startPublisher;
            _iconsProvider = iconsProvider;
            _factory = factory;
            _config = config;
            _storage = storage;
            _defaultItemPrefab = defaultItemPrefab;
            _infoHint = infoHint;
            _staticDataService = staticDataService;
        }

        protected override ShopItemEntity CreateInternal(ShopItemEntityFactoryArgs args)
        {
            ShopItemView view = Object.Instantiate(_defaultItemPrefab, args.Parent);
            ShopItemEntity entity = new ShopItemEntity(view, _storage, _factory, _config, _iconsProvider,
                _startPublisher, _finishPublisher, _infoHint,_staticDataService);
            view.gameObject.name = view.gameObject.name.Replace("(Clone)", $"_{_index}");
            _index++;
            entity.Disposed += _ => _entities.Remove(entity);
            _entities.Add(entity);
            return entity;
        }
    }

    public struct ShopItemEntityFactoryArgs
    {
        public Transform Parent { get; }

        public ShopItemEntityFactoryArgs(Transform parent)
        {
            Parent = parent;
        }
    }
}
