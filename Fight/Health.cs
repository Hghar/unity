using System;
using CountablePublishers;
using Parameters;
using UnityEngine;

namespace Fight
{
    public class Health : UnitParam, IHealth
    {
        private int _value; // TODO: Create limited NaturalPublisher for mana
        private NaturalPublisher _max;
        private bool _locked;
        private int _startValue;

        public int Value => _value;
        public ICountablePublisher<int> MaxValuePublisher => _max;

        public event Action BecameZero;
        public event Action<int> DecreasedBy;
        public event Action<int> IncreasedBy;

        public event Action<int> MaxValueIncreasedBy;

        public event Action<int> MaxValueDecreasedBy;
        public event Action<float> HPValueUpdate;
        
        public Health(int value) : base(ParamType.Health)
        {
            _value = value;
            _startValue = value;
            _max = new NaturalPublisher(value);
            _max.IncreasedBy += OnMaxValueIncreasedBy;
            _max.DecreasedBy += OnMaxValueDecreasedBy;
        }

        public void UpdateParam(int value)
        {
            _startValue = value;
            _max.UpdateParam(value);
        }

        public void Decrease(int delta) // TODO: publish real delta, subscribe damage text on damagable
        {
            if (_value <= 0 || _locked)
                return;

            int decreasedValue = _value - delta;

            if (decreasedValue > _max.Value)
            {
                decreasedValue = _max.Value;
            }

            if (decreasedValue > 0)
            {
                _value = decreasedValue;
                DecreasedBy?.Invoke(delta);
            }
            else
            {
                if (Immortality)
                {
                    _value = 1;
                    DecreasedBy?.Invoke(delta);
                    HPValueUpdate?.Invoke(_value);
                    return;
                }
                _value = 0;

                DecreasedBy?.Invoke(delta);
                BecameZero?.Invoke();
            }

            HPValueUpdate?.Invoke(_value);
        }

        public void Increase(int delta)
        { 
            int increasedValue = _value + delta;

            if (increasedValue < _max.Value)
                _value = increasedValue;
            else
                _value = _max.Value;

            HPValueUpdate?.Invoke(_value);
            IncreasedBy?.Invoke(delta);
        }

        public float FillPercent => (float)Value / _max.Value;
        public float StartValue => _startValue;
        public bool Immortality { get; set; }

        public void IncreaseMax(int value)
        {
            var oldValue = _max.Value;
            _max = new NaturalPublisher(oldValue+value);
            Increase(value);
        }

        public void DecreaseMax(int value)
        {
            var oldValue = _max.Value;
            Decrease(value);
            _max = new NaturalPublisher(oldValue-value);
            if (_value > _max.Value)
                _value = _max.Value;
        }

        public void Lock()
        {
            _locked = true;
        }

        public void Unlock()
        {
            _locked = false;
        }

        protected override void ApplyModificator(IParamModificator modificator)
        {
            int _previousValue = _value;
            //TODO: change back
            float fullness = _value / (float)_max.Value;
            int newMaxValue = (int)modificator.Apply(_max.Value);
            if (_max.TrySetValue(newMaxValue) == false)
                Debug.LogError($"Failed to set max value {newMaxValue}");
            
            _value = (int) (_max.Value * fullness);

            if (_value <= 0)
            {
                BecameZero?.Invoke();
            }

            HPValueUpdate?.Invoke(_value);
        }

        private void OnMaxValueIncreasedBy(int delta)
        {
            MaxValueIncreasedBy?.Invoke(delta);
        }

        private void OnMaxValueDecreasedBy(int delta)
        {
            MaxValueDecreasedBy?.Invoke(delta);
        }
    }
}