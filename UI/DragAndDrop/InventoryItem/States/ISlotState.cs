using UI.DragAndDrop.InventoryItem.Points;

namespace UI.DragAndDrop.InventoryItem.States
{
    public interface ISlotState
    {
        public IConnectPoint Point { get; }

        void Tick();
        void End();
    }
}