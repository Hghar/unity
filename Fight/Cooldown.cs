using System;
using Parameters;
using UnityEngine;

namespace Fight
{
    public class Cooldown : UnitParam, ICooldown
    {
        [SerializeField] PositiveNumberPublisher _value;

        public event Action<float> IncreasedBy;
        public event Action<float> DecreasedBy;

        public float Value => _value.Value;
        public float StartValue { get; }


        public Cooldown(float value) : base(ParamType.Cooldown)
        {
            _value = new PositiveNumberPublisher(value);
            StartValue = value;
            _value.IncreasedBy += OnValueIncreasedBy;
            _value.DecreasedBy += OnValueDecreasedBy;
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
            float newValue = modificator.Apply(_value.Value);
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