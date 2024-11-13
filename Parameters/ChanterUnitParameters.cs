using Model.Economy;

namespace Parameters
{
    public class ChanterUnitParameters : UnitParameters
    {
        private readonly float _targetDamage;

        public ChanterUnitParameters(int health, float damage, int attackRadius, float armor, float cooldown,
            float criticalChance, float criticalDamage, float agility, int healing, float targetDamage, int energy, Level level, CurrencyValuePair sellingSetup) :
                    base(health, damage, attackRadius, armor, cooldown, criticalChance, criticalDamage, agility,
                        healing, energy, level, sellingSetup)
        {
            _targetDamage = targetDamage;
        }

        public float TargetDamage => _targetDamage;
    }
}