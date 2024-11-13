using System;

namespace Picking
{
    public interface IPickingPublisher<T>
    {
        public event Action<T> Picked;
        public event Action<T> Unpicked;
        public event Action<T> Destroying;
    }

    public interface IPickingPublisher
    {
        public event Action Picked;
        public event Action Unpicked;
    }
}