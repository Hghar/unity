namespace Mights
{
    public class DefaultStrategy : IMightStrategy
    {
        public float EffectiveDps(float cooldown, float range, float maxRange, float power, float criticalDamageChance,
            float criticalDamageMultiplier)
        {
            return ((1 / cooldown) * (range / maxRange)) * (power * (1 + criticalDamageChance * criticalDamageMultiplier));
        }

        public float EffectiveHp(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            if(currentHealth == -1)
            {
                currentHealth = maxHealth;
            }

            return currentHealth * (1 / (1 - chanceOfDodge + (1 - chanceOfDodge) * (100 / (100 - armor) - 1)));
        }
    }
}