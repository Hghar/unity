namespace Mights.Units
{
    public class ChanterMight : Might
    {
        protected override void SetEffectiveDPS(float cooldown, float range, float maxRange, float power, float criticalDamageChance, float criticalDamageMultiplier)
        {
            base.SetEffectiveDPS(cooldown, range, maxRange, power, criticalDamageChance, criticalDamageMultiplier);

            _effectiveDPS = ((1 / _characterConfig.TimeBetweenAttacks) * (_characterConfig.Range / _constantsConfig.GeneralMaxRange))
                            * (_characterConfig.Power * (1 + _characterConfig.CriticalDamageChance * _characterConfig.CriticalDamageMultiplier)
                               + (_characterConfig.PowerOfHealing * 1 / _characterConfig.AdditionalParameter * 0.9f)
                               * (1 + _characterConfig.CriticalDamageChance * _characterConfig.CriticalDamageMultiplier));
        }
    }
}
