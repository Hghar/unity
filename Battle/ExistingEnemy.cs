using UnityEngine;
using Zenject;

namespace Battle
{
    public class ExistingEnemy : MonoBehaviour
    {
        [SerializeField] private Enemy _enemy;

        [Inject]
        private void Construct(IEnemiesPool enemiesPool)
        {
            // enemiesPool.TryAdd(_enemy);
        }
    }
}