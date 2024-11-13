using Model.Economy;
using Realization.States.CharacterSheet;
using Parameters;


namespace Units
{
    public class Archer : Minion<ArcherUnitParameters>
    {
        protected override ArcherUnitParameters GetParametersInternal(Character config, CurrencyValuePair selling, Level level)
        {
            return new ArcherUnitParameters
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

        protected override ArcherUnitParameters UpParametersInternal(ArcherUnitParameters unitParameters, Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level)
        {
            return (ArcherUnitParameters)unitParameters.UpdateUnitParameters((int)health, (int)power, (int)power_of_healing, level);
        }
    }
}

