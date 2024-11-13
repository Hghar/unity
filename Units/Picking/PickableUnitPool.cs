using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units.Picking
{
    public class PickableUnitPool : MonoBehaviour, IPickableUnitPool // TODO: mb make pools not-bahaviours
    {
        private readonly HashSet<IPickableUnit> _units = new HashSet<IPickableUnit>();

        public event Action<IPickableUnit> ElementPicked;
        public event Action<IPickableUnit> ElementUnpicked;

        private void OnEnable()
        {
            foreach (IPickableUnit unit in _units)
            {
                UnsubscribeFromUnit(unit);
                SubscribeOnUnit(unit);
            }
        }

        private void OnDisable()
        {
            foreach (IPickableUnit unit in _units)
            {
                UnsubscribeFromUnit(unit);
            }
        }

        public bool TryAdd(IPickableUnit unit)
        {
            bool isAdded = _units.Add(unit);

            if (isAdded)
                SubscribeOnUnit(unit);

            return isAdded;
        }

        public bool IsContains(IPickableUnit element)
        {
            return _units.Contains(element);
        }

        private void OnUnitDestroying(IPickableUnit unit)
        {
            _units.Remove(unit);
            UnsubscribeFromUnit(unit);

            unit.Unpick();
            OnUnitUnpicked(unit);
        }

        private void OnUnitUnpicked(IPickableUnit unit)
        {
            ElementUnpicked?.Invoke(unit);
        }

        private void OnUnitPicked(IPickableUnit unit)
        {
            ElementPicked?.Invoke(unit);
        }

        private void SubscribeOnUnit(IPickableUnit unit)
        {
            unit.Destroying += OnUnitDestroying;
            unit.Picked += OnUnitPicked;
            unit.Unpicked += OnUnitUnpicked;
        }

        private void UnsubscribeFromUnit(IPickableUnit unit)
        {
            unit.Destroying -= OnUnitDestroying;
            unit.Picked -= OnUnitPicked;
            unit.Unpicked -= OnUnitUnpicked;
        }
    }
}