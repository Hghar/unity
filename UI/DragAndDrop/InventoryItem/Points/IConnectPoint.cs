using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.Points
{
    public interface IConnectPoint
    {
        Transform Transform { get; }
        bool IsFree { get; }

        bool IsConnectable(IDraggable draggable);
        IConnectPoint TryConnect(IDraggable draggable);
        IConnectPoint TryDisconnect(IDraggable draggable);
    }
}