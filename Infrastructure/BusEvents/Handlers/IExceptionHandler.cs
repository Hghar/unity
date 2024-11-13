namespace Infrastructure.BusEvents.Handlers
{
    public interface IExceptionHandler : IGlobalSubscriber
    {
        void ThrowException(string exception);
    }
}