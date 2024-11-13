using Fight.Attack.Projectiles;
using Fight.Damaging;
using Fight.Fractions;
using Helpers.Position;
using Point;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public class RangeAttackStrategy : MonoBehaviour, IAttackStrategy
    {
        [SerializeField] private TargetedProjectile _projectilePrefab;
        [SerializeField] private Transform _projectileSpawnPoint;

        private IProjectileFactory _projectileFactory;

        private void Awake()
        {
            _projectileFactory = new ProjectileFactory(_projectilePrefab);
        }

        public void Init(Transform spawnPoint)
        {
            _projectileSpawnPoint = spawnPoint;
        }

        //TODO fix null
        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion minion)
        {
            _projectileFactory.Create(damage, fraction, null, _projectileSpawnPoint.position);
        }

        public MonoBehaviour ToMonoBehaviour()
        {
            return this;
        }
    }
}