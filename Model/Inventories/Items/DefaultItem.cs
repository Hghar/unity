using System;
using Units;
using UnityEngine;

namespace Model.Inventories.Items
{
    public class DefaultItem : IInventoryItem
    {
        private readonly IMinionItem _minionItem;

        public event Action Disposed;

        public string Name { get; }
        public Sprite Sprite => _minionItem.Icon;

        public DefaultItem(string name, IMinionItem minionItem)
        {
            _minionItem = minionItem;
            Name = name;
        }

        public void Dispose()
        {
            Disposed?.Invoke();
        }

        public IMinionItem GetMinionItem()
        {
            return _minionItem;
        }
    }
}