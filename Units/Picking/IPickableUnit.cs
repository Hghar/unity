using Picking;

namespace Units.Picking
{
    public interface IPickableUnit : IUnpickable<IPickableUnit>
    {
        public IUnit Unit { get; }
    }
}