using UI.DragAndDrop.InventoryItem.Points;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.States
{
    public readonly struct StateArgs
    {
        public StateArgs(Transform transform, float timeToDrag, IConnectPointList attachPointList,
            IDraggable draggable)
        {
            Transform = transform;
            TimeToDrag = timeToDrag;
            PointList = attachPointList;
            Draggable = draggable;
        }

        public Transform Transform { get; }
        public float TimeToDrag { get; }
        public IConnectPointList PointList { get; }
        public IDraggable Draggable { get; }
    }
}