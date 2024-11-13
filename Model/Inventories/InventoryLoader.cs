using System.Collections.Generic;
using Entities.InventoryItems;
using Model.Inventories.Items;
using Realization.Configs;
using Units;
using UnityEngine;
using Zenject;

namespace Model.Inventories
{
    public class InventoryLoader : MonoBehaviour
    {
        [SerializeField] private ItemBase _base;

        private InventoryItemEntityFactory _itemEntityFactory;

        [Inject]
        private void Construct(InventoryItemEntityFactory itemEntityFactory)
        {
            _itemEntityFactory = itemEntityFactory;
        }

        private void Awake()
        {
            if (PlayerPrefs.GetInt("level") == 0)
                return;
            string[] items = PlayerPrefs.GetString("inventory_items").Split(',');

            List<IInventoryItem> _items = new List<IInventoryItem>();
            foreach (string item in items)
            {
                if (_base.Find(item) != null)
                {
                    CreateItem(_base.Find(item));
                }
            }
        }

        private void CreateItem(ItemConfig inventoryItem)
        {
            if (inventoryItem == null)
                return;
            ItemConfig config = inventoryItem;

            IMinionItem newMinionItem = new MinionItem(config.Sprite,
                config.Modificators,
                config.Class,
                config.JsonCharacterView,
                config.AttackStrategy);
            DefaultItem newDefaultItem = new DefaultItem(config.Name, newMinionItem);

            InventoryItemEntityFactoryArgs args = new InventoryItemEntityFactoryArgs(newDefaultItem);
            _itemEntityFactory.Create(args);
        }
    }
}