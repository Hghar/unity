using Model.Economy;
using Realization.States.CharacterSheet;
using Parameters;

namespace Units
{
    public class Healer : Minion<HealerUnitParameters>
    {
        protected override HealerUnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new HealerUnitParameters
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
                config.AdditionalParameter,
                config.Energy,
                level,
                selling
            );
        }

        protected override HealerUnitParameters UpParametersInternal(HealerUnitParameters unitParameters, Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return (HealerUnitParameters)unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}