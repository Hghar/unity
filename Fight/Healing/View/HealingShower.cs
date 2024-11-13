using Parameters;
using Realization.VisualEffects;
using Units;
using UnityEngine;
using Zenject;

namespace Fight.Healing.View
{
    public class HealingShower : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private IHealth _health;
        
        private IVisualEffectService _visualEffectService;
        private IMinion _minion;

        [Inject]
        private void Construct(IVisualEffectService visualEffectService)
        {
            _visualEffectService = visualEffectService;
        }

        private void OnDisable()
        {
            _health.IncreasedBy -= OnHealthIncreased;
        }

        private void OnHealthIncreased(int delta)
        {
            _visualEffectService.Create(VisualEffectType.Heal, _minion);
            // _particleSystem.Play();
        }

        public void SetConfig(IMinion minion, IUnitParameters parameters)
        {
            _minion = minion;
            _health = parameters.Health;
            _health.IncreasedBy += OnHealthIncreased;
        }

        public void UpdateConfig(IUnitParameters parameters)
        {
            _health = parameters.Health;
        }
    }
}