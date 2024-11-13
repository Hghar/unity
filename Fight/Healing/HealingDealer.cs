using System.Collections;
using System.Collections.Generic;
using Fight.Fractions;
using UnityEngine;

namespace Fight.Healing
{
    public class HealingDealer : MonoBehaviour
    {
        [SerializeField] private bool _splashHealing;

        private int _healingValue; // Think about healing like type of damage. Remove duplicates with damageDealer
        private Fraction _fraction;

        private readonly List<IHealable> _healables = new List<IHealable>();

        private void Start()
        {
            StartCoroutine(MakeHealing());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IHealable healable))
            {
                if (healable.Fraction == _fraction)
                {
                    if (_splashHealing == false && _healables.Count == 1)
                        return;

                    if (_healables.Contains(healable) == false)
                        _healables.Add(healable);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IHealable damageable))
            {
                if (damageable.Fraction == _fraction)
                {
                    if (_healables.Contains(damageable))
                        _healables.Remove(damageable);
                }
            }
        }

        public void Init(int healingValue, Fraction fraction)
        {
            _healingValue = healingValue;
            _fraction = fraction;
        }

        private IEnumerator MakeHealing()
        {
            yield return new WaitForFixedUpdate();

            for (int i = 0; i < _healables.Count; i++)
            {
                IHealable healable = _healables[i];
                healable.Heal(_healingValue);
            }

            Destroy(gameObject);
        }
    }
}