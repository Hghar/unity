using Entities.InventoryItems;
using Model.Inventories.Items;
using Realization.Configs;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.Inventories
{
    public class StartInventoryFiller : MonoBehaviour
    {
        private StartItemsConfig _startItemsConfig;
        private InventoryItemEntityFactory _inventoryItemEntityFactory;

        [Inject]
        private void Construct(StartItemsConfig startItemsConfig, InventoryItemEntityFactory inventoryItemEntityFactory)
        {
            _startItemsConfig = startItemsConfig;
            _inventoryItemEntityFactory = inventoryItemEntityFactory;
        }

        private void Start()
        {
            foreach (ItemConfig config in _startItemsConfig.Items)
            {
                IMinionItem newMinionItem = new MinionItem(config.Sprite,
                    config.Modificators,
                    config.Class,
                    config.JsonCharacterView,
                    config.AttackStrategy);
                DefaultItem newDefaultItem = new DefaultItem(config.Name, newMinionItem);

                InventoryItemEntityFactoryArgs args = new InventoryItemEntityFactoryArgs(newDefaultItem);
                _inventoryItemEntityFactory.Create(args);
                // TODO: make item creating more simple
            }
        }
    }
}