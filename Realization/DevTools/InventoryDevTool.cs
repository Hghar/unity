using Entities.InventoryItems;
using Infrastructure.Shared.Extensions;
using Model.Inventories;
using Model.Inventories.Items;
using NaughtyAttributes;
using Realization.Configs;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.DevTools
{
    public class InventoryDevTool : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Sprite _test;
        [SerializeField] private ItemConfig[] _configs;

        private InventoryItemEntityFactory _itemEntityFactory;
        private IInventory _inventory;

        [Inject]
        private void Construct(InventoryItemEntityFactory itemEntityFactory, IInventory inventory)
        {
            _inventory = inventory;
            _itemEntityFactory = itemEntityFactory;
        }

        [Button]
        private void AddItem()
        {
            ItemConfig config = _configs.Random();

            // TODO: replace item creating
            // TODO: remove duplicates with ShopBehaviour
            IMinionItem newMinionItem = new MinionItem(config.Sprite,
                config.Modificators,
                config.Class,
                config.JsonCharacterView,
                config.AttackStrategy);
            DefaultItem newDefaultItem = new DefaultItem(config.Name, newMinionItem);

            _itemEntityFactory.Create(new InventoryItemEntityFactoryArgs(newDefaultItem));
        }

        [Button]
        private void RemoveItem()
        {
            _inventory.Remove(_inventory.Items[0]);
        }
    }
}