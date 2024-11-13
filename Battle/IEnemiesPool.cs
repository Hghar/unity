using Units;

namespace Battle
{
    public interface IEnemiesPool
    {
        public bool TryAdd(IMinion enemy);
    }
}