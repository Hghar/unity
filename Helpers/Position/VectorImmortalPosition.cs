using UnityEngine;

namespace Helpers.Position
{
    public class VectorImmortalPosition : IImmortalPosition
    {
        private Vector2 _vector;

        public VectorImmortalPosition(Vector2 vector)
        {
            _vector = vector;
        }

        public Vector2 WorldPosition => _vector;

        public Vector2 Get() => WorldPosition;
    }
}