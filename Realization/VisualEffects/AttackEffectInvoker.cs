using System;
using Fight.Attack;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.VisualEffects
{
    public class AttackEffectInvoker : MonoBehaviour
    {
        [SerializeField] private Attacker _attacker;
        
        private IVisualEffectService _visualEffectService;

        [Inject]
        private void Construct(IVisualEffectService visualEffectService)
        {
            _visualEffectService = visualEffectService;
        }

        private void OnEnable()
        {
            _attacker.Attacked += OnAttacked;
            _attacker.Missed += OnMissed;
        }

        private void OnDisable()
        {
            _attacker.Attacked += OnAttacked;
            _attacker.Missed += OnMissed;
        }

        private void OnMissed(IMinion target)
        {
            _visualEffectService.Create(VisualEffectType.Evade, target);
        }

        private void OnAttacked(IMinion target, float damage, bool critical, int difference)
        {
            if (critical)
            {
                _visualEffectService.Create(VisualEffectType.CriticalDamage, target);
            }
            else
            {
                _visualEffectService.Create(VisualEffectType.Damage, target);
            }
        }
    }
}