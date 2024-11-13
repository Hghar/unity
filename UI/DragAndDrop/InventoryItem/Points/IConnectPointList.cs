using System;

namespace UI.DragAndDrop.InventoryItem.Points
{
    public interface IConnectPointList
    {
        IConnectPoint ClosestTo(IDraggable draggable);
        IConnectPoint ConnectToClosest(IDraggable draggable);
        IConnectPoint ConnectTo(Type type, IDraggable draggable);
    }
}