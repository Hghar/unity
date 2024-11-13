using UI.DragAndDrop.InventoryItem.Points;

namespace UI.DragAndDrop.InventoryItem.States
{
    public class ClickedState : ISlotState
    {
        public IConnectPoint Point { get; }

        public ClickedState(IConnectPoint lastPoint)
        {
            Point = lastPoint;
        }

        public void Tick()
        {
        }

        public void End()
        {
        }
    }
}