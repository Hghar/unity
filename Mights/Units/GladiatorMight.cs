namespace Mights.Units
{
    public class GladiatorMight : Might
    {
        protected override void SetEffectiveDPS(float cooldown, float range, float maxRange, float power, float criticalDamageChance, float criticalDamageMultiplier)
        {
            base.SetEffectiveDPS(cooldown, range, maxRange, power, criticalDamageChance, criticalDamageMultiplier);

            _effectiveDPS = ((1 / cooldown) * (range / maxRange)) * (power * (1 + _characterConfig.AdditionalParameter * 0.7f + criticalDamageChance * criticalDamageMultiplier));
        }
    }
}
