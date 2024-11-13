using System;
using Units;
using UnityEngine;

namespace Model.Inventories.Items
{
    public class MockItem : IInventoryItem
    {
        public event Action Disposed;

        public string Name => "MockItem";
        public Sprite Sprite => null;

        public void Dispose()
        {
            Disposed?.Invoke();
        }

        public IMinionItem GetMinionItem()
        {
            throw new NotImplementedException();
        }
    }
}