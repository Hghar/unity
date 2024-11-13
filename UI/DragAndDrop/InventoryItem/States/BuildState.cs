using UI.DragAndDrop.InventoryItem.Points;
using UnityEngine;

namespace UI.DragAndDrop.InventoryItem.States
{
    public class BuildState : ISlotState
    {
        private readonly StateArgs _args;
        private readonly IConnectPoint _point;

        public IConnectPoint Point { get; }

        public BuildState(StateArgs args)
        {
            _args = args;
            _point = _args.PointList.ConnectTo(typeof(BuildPoint), _args.Draggable);
            if (_point == null)
                _args.PointList.ConnectTo(typeof(InventoryPoint), _args.Draggable);
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