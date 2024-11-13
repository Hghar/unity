using System;
using Realization.Configs;
using Realization.States.CharacterSheet;
using Units;
using Constants = Realization.Configs.Constants;

namespace Mights
{
    public class MightCalculator
    {
        private Character _character;
        private IMightStrategy _strategy;
        
        public MightCalculator(Character character, Constants constants)
        {
            switch (character.Class)
            {
                case MinionClass.Gladiator:
                    _strategy = new GladiatorStrategy(character);
                    break;
                case MinionClass.Templar:
                    _strategy = new DefaultStrategy();
                    break;
                case MinionClass.Cleric:
                    _strategy = new ClericStrategy(_character, constants);
                    break;
                case MinionClass.Chanter:
                    _strategy = new ChanterStrategy(_character, constants);
                    break;
                case MinionClass.Sorcerer:
                    _strategy = new DefaultStrategy();
                    break;
                case MinionClass.Spiritmaster:
                    break;
                case MinionClass.Ranger:
                    _strategy = new DefaultStrategy();
                    break;
                case MinionClass.Assassin:
                    _strategy = new DefaultStrategy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public float EffectiveDPS(float cooldown, float range, float maxRange, float power, 
            float criticalDamageChance, float criticalDamageMultiplier)
        {
            return _strategy.EffectiveDps(cooldown, range, maxRange, power, criticalDamageChance, criticalDamageMultiplier);
        }

        public float EffectiveHP(float maxHealth, float currentHealth, float chanceOfDodge, float armor)
        {
            return _strategy.EffectiveHp(maxHealth, currentHealth, chanceOfDodge, armor);
        }
    }
}