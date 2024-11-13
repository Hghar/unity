using UI.DragAndDrop.InventoryItem.Points;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.States
{
    public class InventoryState : ISlotState
    {
        private readonly StateArgs _args;
        private readonly IConnectPoint _point;

        public IConnectPoint Point { get; }

        public InventoryState(StateArgs args)
        {
            _args = args;
            _point = _args.PointList.ConnectTo(typeof(InventoryPoint), _args.Draggable);
            _args.Transform.SetParent(_point.Transform);
            _args.Transform.localPosition = Vector3.zero;
            (_point as InventoryPoint)?.UpdateLayer();
            Point = _point;
        }

        public void Tick()
        {
        }

        public void End()
        {
            _point.TryDisconnect(_args.Draggable);
        }
    }
}