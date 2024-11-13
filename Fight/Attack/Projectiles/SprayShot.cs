using System.Collections.Generic;
using System.Linq;
using Fight.Damaging;
using Fight.Fractions;
using UnityEngine;

namespace Fight.Attack.Projectiles
{
    public class SprayShot : MonoBehaviour
    {
        private List<TargetlessProjectile> _projectiles;

        private void Awake()
        {
            _projectiles = GetComponentsInChildren<TargetlessProjectile>().ToList();

            foreach (TargetlessProjectile projectile in _projectiles)
            {
                projectile.Destroying += OnProjectileDestroying;
            }
        }

        private void OnDestroy()
        {
            foreach (TargetlessProjectile projectile in _projectiles)
            {
                projectile.Destroying -= OnProjectileDestroying;
            }
        }

        public void Init(IDamage damage, Fraction fraction)
        {
            foreach (TargetlessProjectile projectile in _projectiles)
            {
                projectile.Init(damage, fraction);
            }
        }

        private void OnProjectileDestroying(TargetlessProjectile projectile)
        {
            projectile.Destroying -= OnProjectileDestroying;
            _projectiles.Remove(projectile);
            if (_projectiles.Count() == 0)
                Destroy(gameObject);
        }
    }
}