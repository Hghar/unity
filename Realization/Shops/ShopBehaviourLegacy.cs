using Entities.InventoryItems;
using Entities.ShopItems;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using Model.Economy;
using NaughtyAttributes;
using Realization.Configs;
using UnityEngine;
using Zenject;

namespace Realization.Shops
{
    public class ShopBehaviourLegacy : MonoBehaviour
    {
        [SerializeField] private ItemConfig[] _configs;

        private InventoryItemEntityFactory _itemEntityFactory;
        private IStorage _storage;
        private ShopItemEntityFactory _shopItemEntityFactory;
        private Composite<IRepresentation> _representation;

        [Inject]
        private void Construct(InventoryItemEntityFactory itemEntityFactory, IStorage storage,
            ShopItemEntityFactory shopItemEntityFactory, Composite<IRepresentation> representation)
        {
            _representation = representation;
            _shopItemEntityFactory = shopItemEntityFactory;
            _storage = storage;
            _itemEntityFactory = itemEntityFactory;
        }
        
        [Button]
        private void AddRandomUnit()
        {
            // ItemConfig config = _configs.Random();
            //
            // // TODO: replace item creating
            // IMinionItem newMinionItem = new MinionItem(config.Sprite,
            //     config.Modificators,
            //     config.Class,
            //     config.JsonCharacterView,
            //     config.AttackStrategy);
            // DefaultItem newDefaultItem = new DefaultItem(config.Name, newMinionItem);
            //
            // InventoryItemEntityFactoryArgs args = new InventoryItemEntityFactoryArgs(newDefaultItem);
            // FactoryWrapper<InventoryItemEntityFactoryArgs> wrapper =
            //     new FactoryWrapper<InventoryItemEntityFactoryArgs>(_itemEntityFactory, args);
            //
            // IShopItem item = new FactoryShopItem(wrapper, _storage, config.Currency, config.Name, config.Price,
            //     config.Sprite);
            // ShopItemEntityFactoryArgs shopItemEntityFactoryArgs = new ShopItemEntityFactoryArgs(item, null);
            // _shopItemEntityFactory.Create(shopItemEntityFactoryArgs);
            // _representation.Select().For<ShopItemEntity>().Do().Represent();
        }
    }
}