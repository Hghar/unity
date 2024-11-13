using Parameters;
using UnityEngine;

namespace Fight.Attack
{
    public class CriticalChance : UnitParam
    {
        private PositiveNumberPublisher _value;

        public float Value => _value.Value;

        public CriticalChance(float value) : base(ParamType.AttackRadius)
        {
            _value = new PositiveNumberPublisher(value);
        }

        protected override void ApplyModificator(IParamModificator modificator)
        {
            float newValue = modificator.Apply(_value.Value);
            if (_value.TrySetValue(newValue) == false)
                Debug.LogError($"Failed to set value {newValue}");
        }

        public void Increase(float value)
        {
            _value.Increase(value);
        }
        
        public void Decrease(float value)
        {
            _value.Decrease(value);
        }
    }
}