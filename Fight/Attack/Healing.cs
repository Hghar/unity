using Parameters;
using UnityEngine;

namespace Fight.Attack
{
    public class Healing : UnitParam
    {
        private PositiveNumberPublisher _value;

        public int Value => (int)_value.Value;

        public Healing(int value) : base(ParamType.AttackRadius)
        {
            _value = new PositiveNumberPublisher(value);
        }

        public void UpdateParam(int value)
        {
            _value = new PositiveNumberPublisher(value);
        }

        protected override void ApplyModificator(IParamModificator modificator)
        {
            float newValue = modificator.Apply(_value.Value);
            if (_value.TrySetValue(newValue) == false)
                Debug.LogError($"Failed to set value {newValue}");
        }
    }
}