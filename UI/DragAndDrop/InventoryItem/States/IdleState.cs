using UI.DragAndDrop.InventoryItem.Points;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.States
{
    public class IdleState : ISlotState
    {
        private readonly StateArgs _args;
        private readonly IConnectPoint _point;

        public IConnectPoint Point { get; }

        public IdleState(StateArgs args)
        {
            _args = args;
            _point = _args.PointList.ConnectToClosest(_args.Draggable);
            (_point as InventoryPoint)?.UpdateLayer();
            _args.Transform.SetParent(_point.Transform);
            _args.Transform.localPosition = Vector3.zero;
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