using UnityEngine;

namespace Helpers.Position
{
    public interface IPosition : IReadOnlyPosition
    {
        void Set(Vector2 value);
        void InvokeCache();
    }
}