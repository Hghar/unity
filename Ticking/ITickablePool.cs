using Realization.NewMovers;

namespace Ticking
{
    public interface ITickablePool
    {
        public bool TryAdd(ITickable tickable);
        public bool TryRemove(ITickable tickable);
    }
}