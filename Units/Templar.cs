using Model.Economy;
using Parameters;
using Realization.States.CharacterSheet;

namespace Units
{
    public class Templar : Minion<TemplarUnitParameters>
    {
        protected override TemplarUnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new TemplarUnitParameters(
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

        protected override TemplarUnitParameters UpParametersInternal(TemplarUnitParameters unitParameters, Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return (TemplarUnitParameters)unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}