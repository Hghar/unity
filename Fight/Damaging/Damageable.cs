using System;
using System.Collections.Generic;
using System.Linq;
using Fight.Attack;
using Fight.Fractions;
using Helpers.Position;
using Model.Commands.Parts;
using Parameters;
using Realization.General;
using UnityEngine;
using Random = UnityEngine.Random;
using Realization.States.CharacterSheet;
using Realization.VisualEffects;
using Units;
using Zenject;

namespace Fight.Damaging
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform _center;
        [SerializeField] private int _priority;
        [SerializeField] private Mortality _mortality;
        [SerializeField] private ControlBars _controlBars;

        private IHealth _health;
        private IArmor _armor;
        private Agility _agility;
        private IMinion _minion;
        private ShieldHolder _shieldsHolder = new();
        private IVisualEffectService _visualEffectService;

        // TODO: Separate to decorators: armor, target/priority, _mortality, agility

        public event Action<IDestroyablePoint> Destroying;

        public Fraction Fraction { get; private set; }
        public int Priority => _priority;

        public Vector2 WorldPosition
        {
            get
            {
                if (_center)
                {
                    return _center.position;
                }
                else
                {
                    return Vector2.zero;
                }
            }
        }

        public bool IsDestroying => _mortality.IsDying;
        public bool IsDamaged => _health.Value < _health.MaxValuePublisher.Value; // TODO: lod
        public int ShieldSumm => _shieldsHolder.Summ;
        public int ShieldCount => _shieldsHolder.Count;

        [Inject]
        private void Construct(IVisualEffectService visualEffectService)
        {
            _visualEffectService = visualEffectService;
        }

        private void OnEnable()
        {
            _mortality.Dying += OnDying;
        }

        private void UpdateBars(int _)
        {
            if(_controlBars != null)
                _controlBars.UpdateHPBar(_health.Value, _health.MaxValue);
        }

        private void OnDisable()
        {
            _mortality.Dying -= OnDying;
            _health.DecreasedBy -= UpdateBars;
            _health.IncreasedBy -= UpdateBars;
            _health.MaxValueIncreasedBy -= UpdateBars;
            _health.MaxValueDecreasedBy -= UpdateBars;
        }

        private void OnDying()
        {
            Destroying?.Invoke(this);
        }

        public void TakeDamage(IDamage damage, out bool isKilling)
        {
            isKilling = false;

            var restDamage = _shieldsHolder.TakeDamage(damage.Value);
            damage = new Damage(restDamage);
            if (ShieldSumm == 0)
            {
                _visualEffectService.EndEffectWithDuration(VisualEffectType.Shield, _minion);
            }
            UpdateShieldBar();
            if(restDamage == 0)
                return;

            float damageArmorDifference = damage.Value * (1-_armor.Value * 0.01f);

            if (damageArmorDifference < 0)
                damageArmorDifference = 0;

            isKilling = _health.Value <= damageArmorDifference;
            Debug.Log($"{_minion.GameObject.name} get damage: {damageArmorDifference}");
            _health.Decrease((int)damageArmorDifference);

            if(_controlBars != null)
                _controlBars.UpdateHPBar(_health.Value, _health.MaxValue);

            if(_health.Value <= 0)
            {
                _controlBars.BarsDisabled();
            }
        }

        private void UpdateShieldBar()
        {
            if (_controlBars != null)
                _controlBars.UpdateShieldBar(_shieldsHolder.Summ, _health.MaxValue);
        }

        public void SetConfig(IMinion minion, IUnitParameters unitParameters, Fraction fraction)
        {
            _minion = minion;
            _health = unitParameters.Health;
            _armor = unitParameters.Armor;
            _agility = unitParameters.Agility;
            Fraction = fraction;
            
            _health.DecreasedBy += UpdateBars;
            _health.IncreasedBy += UpdateBars;
            _health.MaxValueIncreasedBy += UpdateBars;
            _health.MaxValueDecreasedBy += UpdateBars;
        }

        public void UpdateConfig(IUnitParameters unitParameters)
        {
            _health.DecreasedBy -= UpdateBars;
            _health.IncreasedBy -= UpdateBars;
            _health.MaxValueIncreasedBy -= UpdateBars;
            _health.MaxValueDecreasedBy -= UpdateBars;
            
            _health = unitParameters.Health;
            _armor = unitParameters.Armor;
            _agility = unitParameters.Agility;
            
            _health.DecreasedBy += UpdateBars;
            _health.IncreasedBy += UpdateBars;
            _health.MaxValueIncreasedBy += UpdateBars;
            _health.MaxValueDecreasedBy += UpdateBars;
        }

        public Vector2 Get() => WorldPosition;


        public bool TryAddShield(int shieldValue, CommandClass commandClass)
        {
            _shieldsHolder.Add(commandClass, shieldValue);
            if (_shieldsHolder.Summ != 0)
            {
                _visualEffectService.CreateEffectWithDuration(VisualEffectType.Shield, _minion);
            }
            UpdateShieldBar();
            return true;
        }

        public void RemoveShield(CommandClass commandClass)
        {
            _shieldsHolder.Remove(commandClass);
            if (_shieldsHolder.Summ == 0)
            {
                _visualEffectService.EndEffectWithDuration(VisualEffectType.Shield, _minion);
            }
            UpdateShieldBar();
        }

        public bool HasShield(CommandClass commandClass)
            => _shieldsHolder.HasShield(commandClass);

        public void UpdateShield(CommandClass commandClass, int shieldValue)
        {
            _shieldsHolder.UpdateShield(commandClass, shieldValue);
            UpdateShieldBar();
        }

        public void RemoveAddShields()
        {
            int shieldCount = _shieldsHolder.Count;
            _shieldsHolder.RemoveAll();
            for (int i = 0; i < shieldCount; i++)
            {
                _visualEffectService.EndEffectWithDuration(VisualEffectType.Shield, _minion);   
            }
            _visualEffectService.EndEffectWithDurationDirty(VisualEffectType.Shield, _minion);   
            UpdateShieldBar();
        }
    }
}