using System;
using System.Collections.Generic;

namespace UnitSelling.Picking
{
    public class SellablePool : ISellablePool
    {
        private readonly HashSet<ISellable> _sellables;

        public event Action<ISellable> ElementPicked;
        public event Action<ISellable> ElementUnpicked;
        public event Action SellableAdded;
        public event Action SellableRemoved;

        public IEnumerable<ISellable> Sellables => _sellables;

        public SellablePool(HashSet<ISellable> sellables = null)
        {
            if (sellables == null)
                _sellables = new HashSet<ISellable>();
            else
                _sellables = sellables;

            foreach (ISellable sellable in _sellables)
            {
                SubscribeOn(sellable);
            }
        }

        public bool TryAdd(ISellable sellable)
        {
            bool isAdded = _sellables.Add(sellable);

            if (isAdded)
            {
                SubscribeOn(sellable);
                SellableAdded?.Invoke();
            }

            return isAdded;
        }

        public bool IsContains(ISellable sellable)
        {
            return _sellables.Contains(sellable);
        }

        private void OnSellableDestroying(ISellable sellable)
        {
            Remove(sellable);
        }

        private void OnSellableUnpicked(ISellable sellable)
        {
            ElementUnpicked?.Invoke(sellable);
        }

        private void OnSellablePicked(ISellable sellable)
        {
            ElementPicked?.Invoke(sellable);
        }

        private void OnSellableSelling(ISellable sellable)
        {
            Remove(sellable);
        }

        private void SubscribeOn(ISellable sellable)
        {
            sellable.Destroying += OnSellableDestroying;
            sellable.Picked += OnSellablePicked;
            sellable.Unpicked += OnSellableUnpicked;
        }

        private void UnsubscribeFrom(ISellable sellable)
        {
            sellable.Destroying -= OnSellableDestroying;
            sellable.Picked -= OnSellablePicked;
            sellable.Unpicked -= OnSellableUnpicked;
        }

        private void Remove(ISellable sellable)
        {
            _sellables.Remove(sellable);
            UnsubscribeFrom(sellable);
            sellable.Unpick();

            SellableRemoved?.Invoke();
            OnSellableUnpicked(sellable);
        }
    }
}