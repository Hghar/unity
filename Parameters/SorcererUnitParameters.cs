using Model.Economy;

namespace Parameters
{
    public class SorcererUnitParameters : UnitParameters
    {
        public SorcererUnitParameters(int health, float damage, int attackRadius, float armor, float cooldown,
            float chanceOfCriticalDamage,
            float criticalDamageMultiplier, float agility, int healing, int energy, Level level, CurrencyValuePair sellingSetup)
            : base(health, damage, attackRadius, armor, cooldown, chanceOfCriticalDamage, criticalDamageMultiplier,
                agility, healing, energy, level, sellingSetup)
        {
        }
    }
}