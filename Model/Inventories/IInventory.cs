using System.Collections.Generic;
using Model.Inventories.Items;

namespace Model.Inventories
{
    public interface IInventory
    {
        IReadOnlyList<IInventoryItem> Items { get; }

        bool TryAdd(IInventoryItem item);
        void Remove(IInventoryItem item);
        void Save();
    }
}