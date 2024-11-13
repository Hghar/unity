using System;
using UnityEngine;

namespace Model.Economy
{
    [Serializable]
    public struct CurrencyValuePair
    {
        [SerializeField] private int[] _value;
        [SerializeField] private Currency _currency;

        public CurrencyValuePair(int[] value, Currency currency)
        {
            _value = value;
            _currency = currency;
        }

        public int[] Value => _value;
        public Currency Currency => _currency;
    }
}