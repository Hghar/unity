using System;
using System.Collections;
using Fight.Damaging;
using Fight.Fractions;
using UnityEngine;

namespace Fight.Attack.Projectiles
{
    public class TargetlessProjectile : MonoBehaviour
    {
        [SerializeField] private InitableDamageDealerFactory _damageDealerFactory;
        [SerializeField] [Min(0)] private float _lifetime;

        public event Action<TargetlessProjectile> Destroying;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(_lifetime);
            Hit();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.Fraction != _damageDealerFactory.Fraction)
                {
                    Hit();
                }
            }
        }

        private void OnDestroy()
        {
            Destroying?.Invoke(this);
        }

        public void Init(IDamage damage, Fraction fraction)
        {
            _damageDealerFactory.Init(damage, fraction, transform);
        }

        private void Hit()
        {
            _damageDealerFactory.Create();
            Destroy(gameObject);
        }
    }
}