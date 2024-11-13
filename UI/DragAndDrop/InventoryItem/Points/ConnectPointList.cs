using System;
using System.Collections.Generic;
using System.Linq;

namespace UI.DragAndDrop.InventoryItem.Points
{
    public class ConnectPointList : IConnectPointList
    {
        private readonly IConnectPoint[] _points;

        public ConnectPointList(IConnectPoint[] points)
        {
            _points = points;
        }

        public IConnectPoint ClosestTo(IDraggable draggable)
        {
            return _points
                .Where(point => point.IsConnectable(draggable))
                .OrderBy(point => (point.Transform.position - draggable.Position).sqrMagnitude)
                .First();
        }

        public IConnectPoint ConnectToClosest(IDraggable draggable)
        {
            IConnectPoint closest = ClosestTo(draggable).TryConnect(draggable);
            return closest;
        }

        public IConnectPoint ConnectTo(Type type, DraggableSlot draggable)
        {
            throw new NotImplementedException();
        }

        public IConnectPoint ConnectTo(Type type, IDraggable draggable)
        {
            List<IConnectPoint> points = _points
                .Where(point => point.GetType() == type)
                .Where(point => point.IsConnectable(draggable)).ToList();

            IConnectPoint fountPoint = null;
            if (points.Count > 0)
                fountPoint = points.OrderBy(point => point.IsFree == false)
                    .ThenBy(point => (point.Transform.position - draggable.Position).magnitude)
                    .First()
                    .TryConnect(draggable);

            return fountPoint ?? ConnectToClosest(draggable);
        }
    }
}