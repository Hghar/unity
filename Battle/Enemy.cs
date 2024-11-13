using System;
using Fight;
using UnityEngine;

namespace Battle
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        [SerializeField] private Mortality _mortality;

        public event Action<IEnemy> Dying;
        public event Action<IEnemy> Destroying;

        private void OnEnable()
        {
            _mortality.Dying += OnDying;
        }

        private void OnDisable()
        {
            _mortality.Dying -= OnDying;
        }

        private void OnDestroy()
        {
            Destroying?.Invoke(this);
        }

        private void OnDying()
        {
            Dying?.Invoke(this);
        }
    }
}