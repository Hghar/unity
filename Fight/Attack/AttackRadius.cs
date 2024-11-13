using Parameters;
using UnityEngine;

namespace Fight.Attack
{
    public class AttackRadius : UnitParam
    {
        private PositiveNumberPublisher _value;

        public float Value => _value.Value;

        public AttackRadius(int value) : base(ParamType.AttackRadius)
        {
            _value = new PositiveNumberPublisher((float) value);
        }

        protected override void ApplyModificator(IParamModificator modificator)
        {
            float newValue = modificator.Apply(_value.Value);
            if (_value.TrySetValue(newValue) == false)
                Debug.LogError($"Failed to set value {newValue}");
        }
    }
}