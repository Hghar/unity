using Realization.Configs;
using Realization.States.CharacterSheet;
using Constants = Realization.Configs.Constants;

namespace Mights
{
    public class SpiritmasterStrategy : IMightStrategy
    {
        private Character _parameters;
        private readonly DefaultStrategy _defaultStrategy;
        private Constants _constants;

        public SpiritmasterStrategy(Character parameters, Constants constants)
        {
            _constants = constants;
            _parameters = parameters;
            _defaultStrategy = new DefaultStrategy();
        }
        
        public float EffectiveDps(float cooldown, float range, float maxRange, float power, float criticalDamageChance,
            float criticalDamageMultiplier)
        {
            return _defaultStrategy.EffectiveDps(cooldown, range, maxRange, power, criticalDamageChance,
                criticalDamageMultiplier);
        }

        public float EffectiveHp(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            return currentHealth 
                   * (1 / (1 - chanceOfDodge + (1 - chanceOfDodge) * (100 / (100 - armor) - 1) + 
                           (((1 / _parameters.TimeBetweenAttacks)
                             * (_parameters.Range / (float)_constants.GeneralMaxRange)) * 
                            (_parameters.Power * (1 + _parameters.CriticalDamageChance * _parameters.CriticalDamageMultiplier)) 
                            * 0.9f * _parameters.AdditionalParameter)));
        }
    }
}