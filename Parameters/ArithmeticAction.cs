using System;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class ArithmeticAction : IArithmeticAction
    {
        [SerializeField] [Min(1)] private float _value;
        [SerializeField] private ArithmeticOperationType _operation;

        public float Value => _value;
        public ArithmeticOperationType Operation => _operation;

        public ArithmeticAction(float value, ArithmeticOperationType operation)
        {
            _value = value;
            _operation = operation;
        }

        public float Apply(float previousNumber)
        {
            switch (_operation)
            {
                case ArithmeticOperationType.Addition:
                    return previousNumber + _value;
                case ArithmeticOperationType.Division:
                    return previousNumber / _value;
                case ArithmeticOperationType.Multiplication:
                    return previousNumber * _value;
                case ArithmeticOperationType.Subtraction:
                    return previousNumber - _value;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public enum ArithmeticOperationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }
}