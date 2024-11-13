using Parameters;
using UnityEngine;

namespace Fight.Attack
{
    public class Energy : UnitParam
    {
        private PositiveNumberPublisher _value;

        public float Value => _value.Value;
        public float MaxValue { get; private set; }

        public Energy(float value) : base(ParamType.Energy)
        {
            _value = new PositiveNumberPublisher(value);
            MaxValue = value;
        }

        protected override void ApplyModificator(IParamModificator modificator)
        {
            float newValue = modificator.Apply(_value.Value);
            if (_value.TrySetValue(newValue) == false)
                Debug.LogError($"Failed to set value {newValue}");
        }
    }
}
