using System;

namespace CountablePublishers
{
    public interface ILimitedCountablePublisher<T> : ICountablePublisher<T>
    {
        public event Action<T> MaxValueIncreasedBy;
        public event Action<T> MaxValueDecreasedBy;

        public ICountablePublisher<T> MaxValuePublisher { get; }

        public virtual T MaxValue => MaxValuePublisher.Value;
    }
}