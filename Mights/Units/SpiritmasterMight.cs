namespace Mights.Units
{
    public class SpiritmasterMight : Might
    {
        protected override void SetEffectiveHP(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            base.SetEffectiveHP(maxHealth, currentHealth, chanceOfDodge, armor);

            _effectiveHP = currentHealth * (1 / (1 - chanceOfDodge + (1 - chanceOfDodge) * (100 / (100 - armor) - 1) + (((1 / _characterConfig.TimeBetweenAttacks)
                * (_characterConfig.Range / _constantsConfig.GeneralMaxRange)) * (_characterConfig.Power
                * (1 + _characterConfig.CriticalDamageChance * _characterConfig.CriticalDamageMultiplier)) * 0.9f * _characterConfig.AdditionalParameter)));
        }
    }
}
