using System;
using CountablePublishers;

namespace Fight
{
    [Serializable]
    public class NaturalPublisher : ICountablePublisher<int> // TODO: Use natural number calss
    {
        private int _value;

        public int Value => _value;

        public event Action<int> IncreasedBy;
        public event Action<int> DecreasedBy;

        public NaturalPublisher(int value)
        {
            _value = value;
        }

        public void UpdateParam(int value)
        {
            _value = value;
        }

        public void Decrease(int delta)
        {
            int decreasedValue = _value - delta;
            if (decreasedValue > 0)
            {
                _value = decreasedValue;
                DecreasedBy?.Invoke(delta);
            }
            else
            {
                _value = 0;
                DecreasedBy?.Invoke(delta); // TODO: publish real delta. For damage showing use damаge.
            }
        }

        public void Increase(int delta)
        {
            int increasedValue = _value + delta;
            IncreasedBy?.Invoke(delta);
        }

        public bool TrySetValue(int newValue)
        {
            if (newValue < 0)
                return false;

            int _previousValue = _value;
            _value = newValue;

            if (_previousValue > _value)
                DecreasedBy?.Invoke(_previousValue - _value);

            if (_previousValue < _value)
                IncreasedBy?.Invoke(_value - _previousValue);

            return true;
        }
    }

    //todo refactor and use generic
}