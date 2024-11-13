using System;

namespace Infrastructure.CompositeDirector.Executors
{
    public interface IProcessExecutor : IDisposable
    {
        event Action<IProcessExecutor> Disposed;
    }
}