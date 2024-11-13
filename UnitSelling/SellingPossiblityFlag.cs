using System;
using System.Linq;
using UnitSelling.Picking;
using UnityEngine;
using Zenject;

namespace UnitSelling
{
    public class SellingPossiblityFlag : ISellingPossiblityFlag, IDisposable
    {
        private const int MinimalSellablesAmount = 1;

        private bool _value;
        private ISellablePool _pool;

        public bool Value => _value;

        public event Action ValueChanged;

        [Inject]
        private void Construct(ISellablePool pool)
        {
            _pool = pool;
            _pool.SellableAdded += OnSellableAdded;
            _pool.SellableRemoved += OnSellableRemoved;
        }

        public void Dispose()
        {
            _pool.SellableAdded -= OnSellableAdded;
            _pool.SellableRemoved -= OnSellableRemoved;
        }

        private void OnSellableRemoved()
        {
            UpdateValue();
        }

        private void OnSellableAdded()
        {
            UpdateValue();
        }

        private void UpdateValue()
        {
            bool previousValue = _value;
            _value = _pool.Sellables.Count() > MinimalSellablesAmount;

            if (previousValue != _value)
                ValueChanged?.Invoke();
        }
    }
}