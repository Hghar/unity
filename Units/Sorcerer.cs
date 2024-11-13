using Model.Economy;
using Realization.States.CharacterSheet;
using Parameters;


namespace Units
{
    public class Sorcerer : Minion<SorcererUnitParameters>
    {
        protected override SorcererUnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new SorcererUnitParameters
            (
                config.Health,
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
                selling
            );
        }

        protected override SorcererUnitParameters UpParametersInternal(SorcererUnitParameters unitParameters, Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return (SorcererUnitParameters)unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}

