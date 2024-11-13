using System;
using Zenject;

namespace Picking
{
    public abstract class Picker<TPool, TUnpickable> : IDisposable
        where TUnpickable : IUnpickable<TUnpickable>
        where TPool : IPickablePool<TUnpickable>
    {
        private TPool _pool;
        private TUnpickable _picked;
        private bool _hasPicked;

        public event Action<TUnpickable> ElementPicked;
        public event Action<TUnpickable> ElementUnpicked;

        [Inject]
        private void Construct(TPool pool)
        {
            _pool = pool;
            _pool.ElementPicked += OnPoolElementPicked;
            _pool.ElementUnpicked += OnPoolElementUnpicked;
            OnConstructing();
        }

        public bool TryGetPicked(out TUnpickable picked)
        {
            picked = _picked;
            return _hasPicked;
        }

        public void Dispose()
        {
            if (_pool != null)
            {
                _pool.ElementPicked -= OnPoolElementPicked;
                _pool.ElementUnpicked -= OnPoolElementUnpicked;
            }
            OnDisposing();
        }

        protected virtual void OnDisposing()
        {
        }

        protected virtual void OnConstructing()
        {
        }

        private void OnPoolElementPicked(TUnpickable newPicked)
        {
            if (_hasPicked)
            {
                if (_picked.Equals(newPicked))
                    return;

                _picked.Unpick();
            }

            _hasPicked = true;
            _picked = newPicked;
            ElementPicked?.Invoke(_picked);
        }

        private void OnPoolElementUnpicked(TUnpickable unpickedContent)
        {
            if ((_picked != null) && (_picked.Equals(unpickedContent) == false))
                return;

            _hasPicked = false;
            ElementUnpicked?.Invoke(unpickedContent);
        }
    }
}