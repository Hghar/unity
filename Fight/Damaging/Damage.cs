using System;
using Parameters;
using UnityEngine;

namespace Fight.Damaging
{
    public class Damage : UnitParam, IDamage
    {
        private NaturalFloatPublisher _value;

        private float _startValue;

        public event Action<float> IncreasedBy;

        public event Action<float> DecreasedBy;

        public float Value => _value.Value;

        public Damage(float value) : base(ParamType.Damage)
        {
            _startValue = value;
            _value = new NaturalFloatPublisher(value);
            _value.IncreasedBy += OnValueIncreasedBy;
            _value.DecreasedBy += OnValueDecreasedBy;
        }

        public void UpdateParam(int value)
        {
            _startValue = value;
            _value = new NaturalFloatPublisher(value);
        }

        public void Decrease(float delta)
        {
            _value.Decrease(delta);
        }

        public void Increase(float delta)
        {
            _value.Increase(delta);
        }
        
        protected override void ApplyModificator(IParamModificator modificator)
        {
            float newValue = (float) modificator.Apply(_value.Value);
            if (_value.TrySetValue(newValue) == false)
                Debug.LogError($"Failed to set value {newValue}");
        }

        private void OnValueIncreasedBy(float delta)
        {
            IncreasedBy?.Invoke(delta);
        }

        private void OnValueDecreasedBy(float delta)
        {
            DecreasedBy?.Invoke(delta);
        }

        public float StartValue => _startValue;
    }
}