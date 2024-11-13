using System;

namespace CountablePublishers
{
    public interface ICountablePublisher<T>
    {
        public event Action<T> IncreasedBy;
        public event Action<T> DecreasedBy;

        public T Value { get; }
        
        void Decrease(T delta);
        void Increase(T delta);
    }
}