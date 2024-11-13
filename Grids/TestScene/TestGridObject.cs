using UnityEngine;

namespace Grids
{
    public class TestGridObject : IGridObject
    {
        private TestGridObjectBehaviour _behaviour;
        private Vector2Int _position;

        public TestGridObject(TestGridObjectBehaviour behaviour)
        {
            _behaviour = behaviour;
        }
        public string Name { get; }

        public Vector2Int Position
        {
            get => _position;
            set
            {
                _behaviour.SetOnPosition(value);
                _position = value;
            }
        }
    }
}