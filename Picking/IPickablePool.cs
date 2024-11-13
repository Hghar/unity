using System;

namespace Picking
{
    public interface IPickablePool<T> where T : IUnpickable<T>
    {
        public event Action<T> ElementPicked;
        public event Action<T> ElementUnpicked;

        public bool TryAdd(T newElement);
        public bool IsContains(T element);
    }
}