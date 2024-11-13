using System;
using Entities.InventoryItems.Views;
using Infrastructure.CompositeDirector.Executors;
using Model.Composites.Representation;
using Model.Inventories.Items;
using UnityEngine;

namespace Entities.InventoryItems
{
    public class InventoryItemEntity : IRepresentation
    {
        private readonly IInventoryItem _item;
        private InventoryItemView _view;

        public event Action<IProcessExecutor> Disposed;

        public InventoryItemEntity(IInventoryItem item, InventoryItemView view)
        {
            item.Disposed += Dispose;
            _item = item;
            _view = view;
        }

        public Transform Transform => _view.transform;

        public void Represent()
        {
            // TODO: group when the same item
            _view.InitDraggable(_item);
            _view.Rename(_item.Name);
            _view.ChangeSprite(_item.Sprite);
        }

        public void Dispose()
        {
            if (_view.Equals(null) == false)
            {
                _view.Dispose();
                _item.Disposed -= Dispose;
                _view = null;
            }

            Disposed?.Invoke(this);
        }

        public T AddComponent<T>() where T : Component
        {
            return _view.gameObject.AddComponent<T>();
        }
    }
}