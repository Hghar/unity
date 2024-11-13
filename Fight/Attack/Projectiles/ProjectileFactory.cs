using Fight.Damaging;
using Fight.Fractions;
using Helpers.Position;
using UnityEngine;

namespace Fight.Attack.Projectiles
{
    public class ProjectileFactory : IProjectileFactory
    {
        [SerializeField] private TargetedProjectile _projectilePrefab;

        public ProjectileFactory(TargetedProjectile projectilePrefab)
        {
            _projectilePrefab = projectilePrefab;
        }

        public void Create(IDamage damage, Fraction fraction, IDestroyablePoint target, Vector2 position)
        {
            TargetedProjectile projectile = Object.Instantiate(_projectilePrefab, position, Quaternion.identity);
            projectile.Init(damage, fraction, target);
        }
    }
}