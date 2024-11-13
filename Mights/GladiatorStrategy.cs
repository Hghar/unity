using Parameters;
using Realization.States.CharacterSheet;

namespace Mights
{
    public class GladiatorStrategy : IMightStrategy
    {
        private Character _parameters;
        private readonly DefaultStrategy _defaultStrategy;

        public GladiatorStrategy(Character parameters)
        {
            _parameters = parameters;
            _defaultStrategy = new DefaultStrategy();
        }
        
        public float EffectiveDps(float cooldown, float range, float maxRange, float power, float criticalDamageChance,
            float criticalDamageMultiplier)
        {
            return ((1 / cooldown) * (range / maxRange)) * 
                   (power * (1 + _parameters.AdditionalParameter * 0.7f + 
                             criticalDamageChance * criticalDamageMultiplier));
        }

        public float EffectiveHp(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            return _defaultStrategy.EffectiveHp(maxHealth, currentHealth, chanceOfDodge, armor);
        }
    }
}