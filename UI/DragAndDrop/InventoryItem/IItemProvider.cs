using Model.Inventories.Items;

namespace UI.DragAndDrop.InventoryItem
{
    public interface IItemProvider
    {
        IInventoryItem Item { get; }
    }
}