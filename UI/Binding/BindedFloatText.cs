using System;
using UnityEngine;

namespace UI.Binding
{
    public class BindedFloatText : BindedText<float>
    {
        [SerializeField] private int _numbersAfterPoint = 1;

        protected override string ConvertValueToString(float value)
        {
            return Math.Round(value, _numbersAfterPoint).ToString();
        }
    }
}