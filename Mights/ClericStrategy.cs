﻿using Realization.Configs;
using Realization.States.CharacterSheet;
using Constants = Realization.Configs.Constants;

namespace Mights
{
    public class ClericStrategy : IMightStrategy
    {
        private Character _parameters;
        private readonly DefaultStrategy _defaultStrategy;
        private Constants _constants;

        public ClericStrategy(Character parameters, Constants constants)
        {
            _constants = constants;
            _parameters = parameters;
            _defaultStrategy = new DefaultStrategy();
        }
        
        public float EffectiveDps(float cooldown, float range, float maxRange, float power, float criticalDamageChance,
            float criticalDamageMultiplier)
        {
            return ((1 / _parameters.TimeBetweenAttacks) * (_parameters.Range / (float)_constants.GeneralMaxRange))
                   * (_parameters.Power * (1 + _parameters.CriticalDamageChance * _parameters.CriticalDamageMultiplier)
                      + (_parameters.PowerOfHealing * 1 / _parameters.AdditionalParameter * 0.9f)
                      * (1 + _parameters.CriticalDamageChance * _parameters.CriticalDamageMultiplier));
        }

        public float EffectiveHp(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            return _defaultStrategy.EffectiveHp(maxHealth, currentHealth, chanceOfDodge, armor);
        }
    }
}