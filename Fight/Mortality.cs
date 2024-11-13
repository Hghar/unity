using System;
using System.Collections;
using DG.Tweening;
using Firebase.Analytics;
using Model.Economy;
using Parameters;
using Units;
using UnityEngine;
using Zenject;

namespace Fight
{
    public class Mortality : MonoBehaviour
    {
        private IHealth _health;
        [SerializeField] private float _delay;

        private bool _isDying = false;
        private IStorage _storage;

        public bool IsDying => _isDying;

        public event Action Dying;

        [Inject]
        private void Construct(IStorage storage)
        {
            _storage = storage;
        }

        private void OnDisable()
        {
            //_health.BecameZero -= OnHealthBecameZero;
        }

        private void OnHealthBecameZero()
        {
            Dying?.Invoke();
            FirebaseAnalytics.LogEvent("unit_died");
            _isDying = true;
            // Destroy(gameObject, _delay);

            StartCoroutine(EffectFade());
        }

        private IEnumerator EffectFade()
        {
            float delay = 0;
            if (_delay > 2)
            {
                yield return new WaitForSeconds(2f);

                delay = _delay - 2;
            }
            else
            {
                delay = _delay;
            }

            SpriteRenderer[] spriteRenderersChildren = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < spriteRenderersChildren.Length; i++)
            {
                spriteRenderersChildren[i].DOFade(0, delay);
            }
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }
        
        public void SetConfig(IUnitParameters parameters)
        {
            _health = parameters.Health;

            _health.BecameZero += OnHealthBecameZero;
        }
        
        public void UpdateConfig(IUnitParameters parameters)
        {
            _health = parameters.Health;
        }

        public void AddCoinsOnDeath()
        {
            _health.BecameZero += AddCoins;
        }

        private void AddCoins()
        {
            _storage.AddResource(Currency.Gold, 35);
        }

    }
}