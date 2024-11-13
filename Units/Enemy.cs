using Model.Economy;
using Parameters;
using Realization.States.CharacterSheet;

namespace Units
{
    public class Enemy : Minion<UnitParameters>
    {
        protected override UnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new UnitParameters(config.Health,
                config.Power,
                config.Range,
                config.Armor,
                config.TimeBetweenAttacks,
                config.CriticalDamageChance,
                config.CriticalDamageMultiplier,
                config.ChanceOfDodge,
                config.PowerOfHealing,
                config.Energy,
                level,
                selling);
        }

        protected override UnitParameters UpParametersInternal(UnitParameters unitParameters ,Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}