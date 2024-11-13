using System;

namespace Helpers.Position
{
    public interface IDestroyablePoint : IReadOnlyPosition
    {
        public bool IsDestroying { get; }

        public event Action<IDestroyablePoint> Destroying;
    }
}