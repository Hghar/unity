using Infrastructure.CompositeDirector.Executors;

namespace Infrastructure.CompositeDirector.CompositeProviders
{
    public interface ICompositeContainer<T> : IServiceContainerExecutor<T> where T : IProcessExecutor
    {
        ICompositeScope<T> For<TSpecific>() where TSpecific : IProcessExecutor;
        ICompositeScope<T> ForAll();
    }
}