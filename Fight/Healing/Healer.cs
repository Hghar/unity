using System;
using System.Collections;
using Fight.Attack;
using Fight.Fractions;
using Fight.Healing.View;
using Fight.Targeting;
using UnityEngine;

namespace Fight.Healing
{
    public class Healer : MonoBehaviour
    {
        // TODO: remove duplicates with attacker
        [SerializeField] private HealingDealer _healingDealerPrefab;
        [SerializeField] private FightTargetFinder _targetFinder;
        [SerializeField] private FractionMarker _fractionMarker;
        [SerializeField] private int _healingValue;
        [SerializeField] private float _cooldown;
        [SerializeField] private HealingRay _healingRay; // TODO: inverse this dependency

        private bool _isHealing;
        private Mortality _mortality;
        private Coroutine _healing;

        private Attacker _attacker;
        // TODO: separeate dependencies

        public bool IsHealing => _isHealing;

        public event Action HealingStarted;
        public event Action HealingStopped;

        private void Awake()
        {
            // TODO: init someway better
            _attacker = GetComponentInParent<Attacker>();
            _mortality = GetComponentInParent<Mortality>();

            if (_attacker != null)
            {
                _attacker.Ready += OnAttackerReady;
                _attacker.Destroyed += OnAttackerDestroyed;
            }
        }

        private void OnEnable()
        {
            _targetFinder.TargetFound += OnTargetFound;

            if (_mortality != null)
                _mortality.Dying += OnDying;

            StartHealing();
        }

        private void OnDisable()
        {
            if (_targetFinder != null)
                _targetFinder.TargetFound -= OnTargetFound;

            if (_mortality != null)
                _mortality.Dying -= OnDying;

            if (_healing != null)
                StopCoroutine(_healing);
            _healingRay.StopShowing();
            HealingStopped?.Invoke();
            _isHealing = false;
        }

        private void OnDestroy()
        {
            if (_attacker != null)
            {
                _attacker.Ready -= OnAttackerReady;
                _attacker.Destroyed -= OnAttackerDestroyed;
            }
        }

        private void OnDying()
        {
            // Debug.LogWarning("There was unsubscribing but i deleted it. Because if disables before target was found");
            _targetFinder.TargetFound -= OnTargetFound;
            _mortality.Dying -= OnDying;
            _attacker.Ready -= OnAttackerReady;
            _attacker.Destroyed -= OnAttackerDestroyed;

            if (_healing != null)
            {
                StopCoroutine(_healing);
                _healingRay.StopShowing();
                HealingStopped?.Invoke();
                _isHealing = false;
            }
        }

        private void OnTargetFound()
        {
            StartHealing();
        }

        private IEnumerator Healing()
        {
            while (_targetFinder.ChosenTarget != null &&
                   (_mortality == null || _mortality.IsDying == false))
            {
                if (_targetFinder.TryGetTargetPosition(out Vector2 targetPosition))
                {
                    HealingDealer healingDealer = Instantiate(_healingDealerPrefab,
                        targetPosition,
                        Quaternion.identity); // TODO: use fabric and container
                    healingDealer.Init(_healingValue, _fractionMarker.Fraction);

                    if (_healingRay.IsShowing == false)
                        _healingRay.ShowRay(transform, _targetFinder.ChosenTarget);

                    HealingStarted?.Invoke();
                    _isHealing = true;

                    yield return new WaitForSeconds(_cooldown);
                }
            }

            _healingRay.StopShowing();
            HealingStopped?.Invoke();
            _isHealing = false;
        }

        private void StartHealing()
        {
            if (_healing != null)
            {
                StopCoroutine(_healing);
                _healingRay.StopShowing();
                HealingStopped?.Invoke();
                _isHealing = false;
            }

            _healing = StartCoroutine(Healing());
        }

        private void OnAttackerDestroyed()
        {
            enabled = false;
        }

        private void OnAttackerReady()
        {
            enabled = true;
        }
    }
}