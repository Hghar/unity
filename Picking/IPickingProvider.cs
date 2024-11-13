namespace Picking
{
    public interface IPickingProvider<T> : IUnpickable<T>
    {
        public void Pick();
        public void Destroy();
    }
}