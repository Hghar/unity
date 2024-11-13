using UnityEngine;

namespace Helpers.Position
{
    public interface IReadOnlyPosition
    {
        public Vector2 WorldPosition { get; }
    }
}