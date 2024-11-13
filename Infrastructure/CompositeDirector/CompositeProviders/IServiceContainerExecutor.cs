using Infrastructure.CompositeDirector.Executors;

namespace Infrastructure.CompositeDirector.CompositeProviders
{
    public interface IServiceContainerExecutor<T> where T : IProcessExecutor
    {
        T Do();
    }
}