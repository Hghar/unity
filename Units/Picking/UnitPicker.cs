using System;
using Picking;
using UnityEngine;

namespace Units.Picking
{
    public class UnitPicker : Picker<IPickableUnitPool, IPickableUnit>, IUnitPicker, IDisposable
    {
        public event Action<IUnit> UnitPicked;
        public event Action UnitUnpicked;

        public bool TryGetUnit(out IUnit unit)
        {
            unit = null;
            bool hasPickable = TryGetPicked(out IPickableUnit pickableUnit);
            if (hasPickable)
            {
                unit = pickableUnit.Unit;
                return true;
            }

            return false;
        }

        protected override void OnConstructing()
        {
            ElementPicked += OnElementPicked;
            ElementUnpicked += OnElementUnpicked;
        }

        protected override void OnDisposing()
        {
            ElementPicked -= OnElementPicked;
            ElementUnpicked -= OnElementUnpicked;
        }

        private void OnElementPicked(IPickableUnit pickable)
        {
            UnitPicked?.Invoke(pickable.Unit);
        }

        private void OnElementUnpicked(IPickableUnit pickable)
        {
            UnitUnpicked?.Invoke();
        }
    }
}