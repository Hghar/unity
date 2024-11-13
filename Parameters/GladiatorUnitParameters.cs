using Model.Economy;
using UnityEngine;

namespace Parameters
{
    public class GladiatorUnitParameters : UnitParameters
    {
        public readonly float AoeDamage;

        public float Personal_might { get; private set; }

        private float _effective_DPS, _effective_HP;

        public GladiatorUnitParameters(int health, float damage, int attackRadius, float armor, float cooldown,
            float criticalChance, float criticalDamage, float agility, int healing, float aoeDamage, int energy, Level level, CurrencyValuePair sellingSetup) :
            base(health, damage, attackRadius, armor, cooldown, criticalChance, criticalDamage, agility,
                healing, energy, level, sellingSetup)
        {
            AoeDamage = aoeDamage;
        }
    }
}