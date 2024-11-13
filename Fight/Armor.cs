using System;
using Parameters;
using UnityEngine;

namespace Fight
{
    public class Armor : UnitParam, IArmor
    {
        [SerializeField] private NaturalFloatPublisher _value;

        public event Action<float> IncreasedBy;

        public event Action<float> DecreasedBy;

        public float Value => _value.Value;
        
        public void Decrease(float delta)
        {
            _value.Decrease(delta);
        }

        public void Increase(float delta)
        {
            _value.Increase(delta);
        }

        public Armor(float value) : base(ParamType.Armor)
        {
            _value = new NaturalFloatPublisher(value);
            _value.IncreasedBy += OnValueIncreasedBy;
            _value.DecreasedBy += OnValueDecreasedBy;
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
    }
}