using System;
using CountablePublishers;
using UnityEngine;

namespace Fight
{
    [Serializable]
    public class NaturalFloatPublisher : ICountablePublisher<float> // TODO: Use natural number calss
    {
        private float _value;

        public float Value => _value;

        public event Action<float> IncreasedBy;
        public event Action<float> DecreasedBy;

        public NaturalFloatPublisher(float value)
        {
            _value = value;
        }

        public void Decrease(float delta)
        {
            float decreasedValue = _value - delta;
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

        public void Increase(float delta)
        {
            _value += delta;
            IncreasedBy?.Invoke(delta);
        }

        public bool TrySetValue(float newValue)
        {
            if (newValue < 0)
                return false;

            float _previousValue = _value;
            _value = newValue;

            if (_previousValue > _value)
                DecreasedBy?.Invoke(_previousValue - _value);

            if (_previousValue < _value)
                IncreasedBy?.Invoke(_value - _previousValue);

            return true;
        }
    }
}