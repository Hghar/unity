using Model.Economy;


namespace Parameters
{
    public class HealerUnitParameters : UnitParameters
    {
        private readonly float _healCount;

        public float HealCount => _healCount;

        public HealerUnitParameters(int health, float damage, int attackRadius, float armor, float cooldown,
            float chanceOfCriticalDamage, float criticalDamageMultiplier, float agility, int healing, float healCount, 
            int energy, Level level,
            CurrencyValuePair sellingSetup)
            : base(health, damage, attackRadius, armor, cooldown, chanceOfCriticalDamage, criticalDamageMultiplier,
                agility,
                healing, energy, level, sellingSetup)
        {
            _healCount = healCount;
        }
    }
}