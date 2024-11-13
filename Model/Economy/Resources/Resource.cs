using System;
using Model.Economy.Numerics;
using UnityEngine;

namespace Model.Economy.Resources
{
    [Serializable]
    public class Resource : IResource
    {
        [SerializeField] private NaturalNumber _value;
        [SerializeField] private Currency _currency;

        public event ValueChanging ValueChanged;        //Не рекомендуется к использованию
        public event Action Changed;

        public Currency Currency => _currency;
        public void Set(int value)
        {
            _value.Set(value);
        }

        public int Value => _value.Value;

        public Resource(NaturalNumber value, Currency currency)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            _currency = currency;
            //_value.ValueChanged += OnChanged;
        }

        public void Add(int addableValue)
        {
            _value.Add(addableValue);
            Changed?.Invoke();
        }

        public void Dispose()
        {
            _value.ValueChanged -= OnChanged;
        }

        public bool CanSubtract(int subtractValue)
        {
            var canSubtract = _value.CanSubtract(subtractValue);
            Changed?.Invoke();

            return canSubtract;
        }

        public bool TrySubtract(int subtractValue)
        {
            var trySubtract = _value.TrySubtract(subtractValue);
            Changed?.Invoke();

            return trySubtract;
        }

        private void OnChanged(int different)
        {
            ValueChanged?.Invoke(different);
        }
    }
}