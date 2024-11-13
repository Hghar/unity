using Model.Economy;
using Parameters;
using Realization.States.CharacterSheet;

namespace Units
{
    public class Gladiator : Minion<GladiatorUnitParameters>
    {
        protected override GladiatorUnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new GladiatorUnitParameters(
                config.Health,
                config.Power,
                config.Range,
                config.Armor,
                config.TimeBetweenAttacks,
                config.CriticalDamageChance,
                config.CriticalDamageMultiplier,
                config.ChanceOfDodge,
                config.PowerOfHealing,
                config.AdditionalParameter,
                config.Energy,
                level,
                selling);
        }

        protected override GladiatorUnitParameters UpParametersInternal(GladiatorUnitParameters unitParameters, Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return (GladiatorUnitParameters)unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}