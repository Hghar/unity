using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Battle
{
    public class EnemiesPool : IEnemiesPool, IAllEnemiesDeadPublisher
    {
        private readonly HashSet<IMinion> _enemies = new();

        public event Action AllEnemiesDead;

        public bool TryAdd(IMinion enemy) // TODO: use common code for list/add/remove
        {
            bool isAdded = _enemies.Add(enemy);
            
            if (isAdded)
                SubscribeToEnemy(enemy);

            return isAdded;
        }

        public bool TryRemove(IMinion enemy)
        {
            bool isRemoved = _enemies.Remove(enemy);

            if (isRemoved)
                UnsubscribeFromEnemy(enemy);

            return isRemoved;
        }

        public void Dispose()
        {
            foreach (IMinion enemy in _enemies)
            {
                UnsubscribeFromEnemy(enemy);
            }

            AllEnemiesDead = null;
        }

        private void OnEnemyDying(IMinion enemy)
        {
            if (enemy.IsBoss)
                AllEnemiesDead = null;

            if (TryRemove(enemy))
            {
                if (_enemies.Count == 0)
                {
                    AllEnemiesDead?.Invoke();
                }
            }
        }

        private void SubscribeToEnemy(IMinion enemy)
        {
            enemy.Died += OnEnemyDying;
        }

        private void UnsubscribeFromEnemy(IMinion enemy)
        {
            enemy.Died -= OnEnemyDying;
        }
    }
}