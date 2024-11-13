namespace Picking
{
    public interface IUnpickable<T> : IPickingPublisher<T>
    {
        public void Unpick();
    }
}