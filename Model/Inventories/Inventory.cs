using System.Collections.Generic;
using System.Linq;
using Model.Inventories.Items;
using UnityEngine;

namespace Model.Inventories
{
    public class Inventory : IInventory
    {
        private readonly List<IInventoryItem> _items = new();

        public IReadOnlyList<IInventoryItem> Items => _items;

        public bool TryAdd(IInventoryItem item)
        {
            if (_items.Contains(item))
                return false;

            _items.Add(item);
            return true;
        }

        public void Remove(IInventoryItem item)
        {
            _items.Remove(item);
            item.Dispose();
        }

        public void Save()
        {
            string json = "";
            foreach (string s in _items.Select((item => item.Name)).ToArray())
            {
                json += $"{s},";
            }

            PlayerPrefs.SetString("inventory_items", json);
        }
    }
}