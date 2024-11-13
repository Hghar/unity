using System;

namespace Battle
{
    public interface IEnemiesSpawnedPublisher
    {
        public event Action EnemiesSpawned;
    }
}