using Fight;
using Parameters;
using Realization.Configs;
using Realization.States.CharacterSheet;
using UnityEngine;
using Constants = Realization.Configs.Constants;

namespace Mights
{
    public class Might : MonoBehaviour
    {
        public float PersonalMight { get; private set; }

        protected float _effectiveDPS;
        protected float _effectiveHP;

        private Health _health;

        protected Character _characterConfig;
        protected Constants _constantsConfig;
        public void SetConfig(Character characterConfig, Constants constantsConfig, Health health)
        {
            _health = health;

            _characterConfig = characterConfig;
            _constantsConfig = constantsConfig;

            SetEffectiveDPS(characterConfig.TimeBetweenAttacks, characterConfig.Range, constantsConfig.GeneralMaxRange, characterConfig.Power, characterConfig.CriticalDamageChance, characterConfig.CriticalDamageMultiplier);
            SetEffectiveHP(characterConfig.Health, -1, characterConfig.ChanceOfDodge, characterConfig.Armor);

            _health.HPValueUpdate += CurrentHPUpdate;

            SetPersonalMight();
        }

        public void UpdateConfig(IUnitParameters config)
        {
            SetEffectiveDPS(_characterConfig.TimeBetweenAttacks, _characterConfig.Range, _constantsConfig.GeneralMaxRange, config.Damage.Value, _characterConfig.CriticalDamageChance, _characterConfig.CriticalDamageMultiplier);
            SetEffectiveHP(_characterConfig.Health, -1, _characterConfig.ChanceOfDodge, _characterConfig.Armor);

            SetPersonalMight();
        }

        private void CurrentHPUpdate(float currentHP)
        {
            if (currentHP == 0)
            {
                _health.HPValueUpdate -= CurrentHPUpdate;
            }

            SetEffectiveHP(_characterConfig.Health, currentHP, _characterConfig.ChanceOfDodge, _characterConfig.Armor);

            SetPersonalMight();
        }

        protected virtual void SetEffectiveDPS(float cooldown, float range, float maxRange, float power, float criticalDamageChance, float criticalDamageMultiplier)
        {
            _effectiveDPS = ((1 / cooldown) * (range / maxRange)) * (power * (1 + criticalDamageChance * criticalDamageMultiplier));
        }

        protected virtual void SetEffectiveHP(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            if(currentHealth == -1)
            {
                currentHealth = maxHealth;
            }

            _effectiveHP = currentHealth * (1 / (1 - chanceOfDodge + (1 - chanceOfDodge) * (100 / (100 - armor) - 1)));
        }
        private void SetPersonalMight()
        {
            PersonalMight = _effectiveDPS * _effectiveHP;
        }
    }
}
