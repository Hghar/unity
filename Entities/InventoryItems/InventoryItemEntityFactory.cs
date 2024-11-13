using System;
using System.Collections.Generic;
using Entities.Factories;
using Entities.InventoryItems.Views;
using Infrastructure.CompositeDirector;
using Model.Inventories;
using Model.Inventories.Items;
using UnityEngine;
using Zenject;

namespace Entities.InventoryItems
{
    public class InventoryItemEntityFactory : EntityFactory<InventoryItemEntity, InventoryItemEntityFactoryArgs>,
        IInventoryItemEntityPool, IDisposable
    {
        private List<InventoryItemEntity> _entities = new();
        public InventoryItemEntity[] Entities => _entities.ToArray();

        private readonly IInventory _inventory;
        private readonly Transform _parent;
        private readonly InventoryItemView _viewPrefab;
        private readonly DiContainer _container;

        public InventoryItemEntityFactory(DiContainer container, CompositeDirector director, IInventory inventory,
            Transform parent, InventoryItemView viewPrefab) : base(director)
        {
            _container = container;
            _inventory = inventory;
            _parent = parent;
            _viewPrefab = viewPrefab;
        }

        protected override InventoryItemEntity CreateInternal(InventoryItemEntityFactoryArgs args)
        {
            _inventory.TryAdd(args.Item);
            InventoryItemView view = _container.InstantiatePrefabForComponent<InventoryItemView>(_viewPrefab, _parent);
            InventoryItemEntity entity = new InventoryItemEntity(args.Item, view);
            _entities.Add(entity);
            return entity;
        }

        public void Dispose()
        {
            _entities = null;
        }
    }

    public struct InventoryItemEntityFactoryArgs
    {
        public IInventoryItem Item { get; }

        public InventoryItemEntityFactoryArgs(IInventoryItem item)
        {
            Item = item;
        }
    }
}