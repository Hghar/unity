using System;

namespace Battle
{
    public interface IAllEnemiesDeadPublisher
    {
        public event Action AllEnemiesDead;
    }
}