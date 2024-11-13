using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Parameters
{
    [Serializable]
    public class ParamModificator : IParamModificator
    {
        [SerializeField] private ArithmeticAction _arithmeticAction;
        [FormerlySerializedAs("_param")] 
        [SerializeField] private ParamType _parameter;

        public ParamType Parameter => _parameter;

        public ParamModificator(ArithmeticAction action, ParamType parameter)
        {
            _arithmeticAction = action;
            _parameter = parameter;
        }

        public float Apply(float previousNumber)
        {
            return _arithmeticAction.Apply(previousNumber);
        }
    }
}