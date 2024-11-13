using System;

namespace Battle
{
    public interface IEnemy
    {
        public event Action<IEnemy> Dying;
        public event Action<IEnemy> Destroying;
    }
}