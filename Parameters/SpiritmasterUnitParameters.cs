using Model.Economy;

namespace Parameters
{
    public class SpiritmasterUnitParameters : UnitParameters
    {
        public readonly float HealDamagePercent;
        
        public SpiritmasterUnitParameters(int health, float damage, int attackRadius, float armor, float cooldown,
            float criticalChance, float criticalDamage, float agility, int healing, float healDamagePercent,
            int energy, Level level, CurrencyValuePair sellingSetup) :
            base(health, damage, attackRadius, armor, cooldown, criticalChance, criticalDamage, agility,
                healing, energy, level, sellingSetup)
        {
            HealDamagePercent = healDamagePercent;
        }
    }
}