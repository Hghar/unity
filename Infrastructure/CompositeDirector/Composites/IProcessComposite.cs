using System.Collections.Generic;
using Infrastructure.CompositeDirector.Executors;

namespace Infrastructure.CompositeDirector.Composites
{
    public interface IProcessComposite : IProcessExecutor
    {
        IReadOnlyList<IProcessExecutor> Items { get; }
        void TryAdd(IProcessExecutor representation);
        void TryRemove(IProcessExecutor representation);
        bool Contains(IProcessExecutor item);
        IProcessComposite Clone();
    }
}