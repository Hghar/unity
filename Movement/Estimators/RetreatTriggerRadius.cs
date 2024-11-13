using Fight;
using Parameters;
using UnityEngine;

namespace Movement.Estimators
{
    public class RetreatTriggerRadius : UnitParam
    {
        private PositiveNumberPublisher _value;

        public RetreatTriggerRadius(float value) : base(ParamType.RetreatTriggerRadius)
        {
            _value = new PositiveNumberPublisher(value);
        }

        public float Value => _value.Value;

        protected override void ApplyModificator(IParamModificator modificator)
        {
            float newValue = modificator.Apply(_value.Value);
            if (_value.TrySetValue(newValue) == false)
                Debug.LogError($"Failed to set value {newValue}");
        }
    }
}