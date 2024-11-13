using Model.Economy;
using Parameters;
using Realization.States.CharacterSheet;

namespace Units
{
    public class Chanter : Minion<ChanterUnitParameters>
    {
        protected override ChanterUnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new ChanterUnitParameters(
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

        protected override ChanterUnitParameters UpParametersInternal(ChanterUnitParameters unitParameters, Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return (ChanterUnitParameters)unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}