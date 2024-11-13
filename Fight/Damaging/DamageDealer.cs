using System.Collections;
using System.Collections.Generic;
using Fight.Fractions;
using UnityEngine;

namespace Fight.Damaging
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] private bool _splashDamage = true;

        private IDamage _damage;
        private Fraction _fraction;

        private readonly List<IDamageable> _damageables = new List<IDamageable>();

        private void Start()
        {
            StartCoroutine(MakeDamage());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.Fraction != _fraction)
                {
                    if (_splashDamage == false && _damageables.Count == 1)
                        return;

                    if (_damageables.Contains(damageable) == false)
                        _damageables.Add(damageable);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.Fraction != _fraction)
                {
                    if (_damageables.Contains(damageable))
                        _damageables.Remove(damageable);
                }
            }
        }

        public void Init(IDamage damage, Fraction fraction)
        {
            _damage = damage;
            _fraction = fraction;
        }

        private IEnumerator MakeDamage()
        {
            yield return new WaitForFixedUpdate();

            for (int i = 0; i < _damageables.Count; i++)
            {
                IDamageable damageable = _damageables[i];
                damageable.TakeDamage(_damage, out bool isKilling);
                if (isKilling)
                {
                    i--;
                    if (_damageables.Contains(damageable))
                        _damageables.Remove(damageable);
                }
            }

            Destroy(gameObject);
        }
    }
}