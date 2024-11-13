using AssetStore.HeroEditor.FantasyInventory.Scripts.Enums;
using Model.Inventories.Items;

namespace UI.DragAndDrop.InventoryItem
{
    public interface IItemDraggable : IDraggable, IItemProvider
    {
        ItemType Type { get; }
        void Init(IInventoryItem item, ItemType type);
        void ReturnToInventory();
        void MoveToDraggableParent();
    }
}