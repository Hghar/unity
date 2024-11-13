using Infrastructure.CompositeDirector.Executors;

namespace Infrastructure.CompositeDirector.CompositeProviders
{
    public interface ICompositeScope<T> : IServiceContainerExecutor<T> where T : IProcessExecutor
    {
        ICompositeScope<T> Select<TSpecific>() where TSpecific : IProcessExecutor;
        ICompositeScope<T> Except<TSpecific>() where TSpecific : IProcessExecutor;
    }
}