using System;
using UnityEngine;

namespace Model.Economy.Numerics
{
    [Serializable]
    public class NaturalNumber  
    {
        [SerializeField]
        private int _value;

        public int Value => _value;

        public event ValueChanging ValueChanged;

        public NaturalNumber(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            _value = value;
        }

        public void Add(int addableValue)
        {
            if (addableValue < 0)
                throw new ArgumentOutOfRangeException(nameof(addableValue));

            int previousValue = _value;
            _value += addableValue;
            if (previousValue > _value)
                _value = int.MaxValue;

            //if (previousValue != _value)
                ValueChanged?.Invoke(addableValue);
        }

        public bool CanSubtract(int subtractValue)
        {
            if (subtractValue < 0)
                throw new ArgumentOutOfRangeException(nameof(subtractValue));

            return _value >= subtractValue;
        }

        public bool TrySubtract(int subtractValue)
        {
            int previousValue = _value;

            if (CanSubtract(subtractValue) == false)
                return false;

            _value -= subtractValue;

            if (previousValue != _value)
                ValueChanged?.Invoke(subtractValue);

            return true;
        }

        public void Set(int value)
        {
            var oldValue = _value;
            _value = value;
            ValueChanged?.Invoke(_value-oldValue);
        }
    }
}