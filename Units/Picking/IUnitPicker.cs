using System;

namespace Units.Picking
{
    public interface IUnitPicker
    {
        public event Action<IUnit> UnitPicked;
        public event Action UnitUnpicked;

        public bool TryGetUnit(out IUnit unit);
    }
}