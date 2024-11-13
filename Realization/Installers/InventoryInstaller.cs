using Entities.InventoryItems;
using Entities.InventoryItems.Views;
using Infrastructure.CompositeDirector;
using Model.Inventories;
using UnityEngine;
using Zenject;

namespace Realization.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private InventoryItemView _itemPrefab;
        [SerializeField] private ItemBase _itemBase;

        private CompositeDirector _director;
        private IInventory _inventory;
        private InventoryItemEntityFactory _entityFactory;

        [Inject]
        private void Construct(CompositeDirector director)
        {
            _director = director;
        }

        public override void InstallBindings()
        {
            _inventory = new Inventory();
            _entityFactory = new InventoryItemEntityFactory(Container, _director, _inventory, _parent, _itemPrefab);

            Container.Bind<IInventoryItemEntityPool>().FromInstance(_entityFactory).AsSingle();
            Container.Bind<InventoryItemEntityFactory>().FromInstance(_entityFactory).AsSingle();
            Container.Bind<IInventory>().FromInstance(_inventory).AsSingle();
        }

        private void OnDestroy()
        {
            _inventory.Save();
            _entityFactory.Dispose();
        }
    }
}