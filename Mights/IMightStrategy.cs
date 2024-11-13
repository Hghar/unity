namespace Mights
{
    public interface IMightStrategy
    {
        float EffectiveDps(
            float cooldown, 
            float range, 
            float maxRange, 
            float power, 
            float criticalDamageChance, 
            float criticalDamageMultiplier);
        
        float EffectiveHp(
            float maxHealth, 
            float currentHealth, 
            float chanceOfDodge, 
            float armor);
    }
}