using System;
using System.Collections.Generic;
using CompositeDirectorWithGeneratingComposites.CompositeDirector.CompositeGeneration;
using Google.MiniJSON;
using Model.Commands.Actions;
using Model.Commands.Parts;
using Realization.States.CharacterSheet;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands.Creation
{
    public class ActionReader
    {
        private IAffectable _composite;
        private MinionFactory _minionFactory;
        private Character[] _characters;
        private IVisualEffectService _effectService;

        public ActionReader(IAffectable composite, MinionFactory minionFactory, Character[] characters,
            IVisualEffectService effectService)
        {
            _effectService = effectService;
            _characters = characters;
            _minionFactory = minionFactory;
            _composite = composite;
        }
        
        public KeyValuePair<Func<Result>, Func<Result>> Read(string action, CommandClass commandClass, params object[] parameters)
        {
            switch (action)
            {
                case "IncreaseHealth":
                    var increaseHealthCommand = new IncreaseHealthCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseHealthCommand),
                        () => _composite.Unperform(increaseHealthCommand));
                case "DecreaseHealth":
                    var decreaseHealthCommand = new DecreaseHealthCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseHealthCommand),
                        () => _composite.Unperform(decreaseHealthCommand));
                case "IncreaseArmor":
                    var increaseArmorCommand = new IncreaseArmorCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseArmorCommand),
                        () => _composite.Unperform(increaseArmorCommand));
                case "DecreaseArmor":
                    var decreaseArmorCommand = new DecreaseArmorCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseArmorCommand),
                        () => _composite.Unperform(decreaseArmorCommand));
                case "IncreasePower":
                    var increasePowerCommand = new IncreasePowerCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increasePowerCommand),
                        () => _composite.Unperform(increasePowerCommand));
                case "DecreasePower":
                    var decreasePowerCommand = new DecreasePowerCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreasePowerCommand),
                        () => _composite.Unperform(decreasePowerCommand));
                case "IncreaseTimeBetweenAttacks":
                    var increaseTimeBetweenAttacksCommand = new IncreaseTimeBetweenAttacksCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseTimeBetweenAttacksCommand),
                        () => _composite.Unperform(increaseTimeBetweenAttacksCommand));
                case "DecreaseTimeBetweenAttacks":
                    var decreaseTimeBetweenAttacksCommand = new DecreaseTimeBetweenAttacksCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseTimeBetweenAttacksCommand),
                        () => _composite.Unperform(decreaseTimeBetweenAttacksCommand));
                case "IncreaseCriticalDamageChance":
                    var increaseCriticalChanceCommand = new IncreaseCriticalChanceCommand(Convert(parameters[0]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseCriticalChanceCommand),
                        () => _composite.Unperform(increaseCriticalChanceCommand));
                case "DecreaseCriticalDamageChance":
                    var decreaseCriticalChanceCommand = new DecreaseCriticalChanceCommand(Convert(parameters[0]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseCriticalChanceCommand),
                        () => _composite.Unperform(decreaseCriticalChanceCommand));
                case "IncreaseCriticalDamageMultiplier":
                    var increaseCriticalMultiplierCommand = new IncreaseCriticalMultiplierCommand((int) parameters[0], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseCriticalMultiplierCommand),
                        () => _composite.Unperform(increaseCriticalMultiplierCommand));
                case "DecreaseCriticalDamageMultiplier":
                    var decreaseCriticalMultiplierCommand = new DecreaseCriticalMultiplierCommand((int) parameters[0],  _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseCriticalMultiplierCommand),
                        () => _composite.Unperform(decreaseCriticalMultiplierCommand));
                case "IncreaseChanceOfDodge":
                    var increaseDodgeChanceCommand = new IncreaseDodgeChanceCommand(Convert(parameters[0]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseDodgeChanceCommand),
                        () => _composite.Unperform(increaseDodgeChanceCommand));
                case "DecreaseChanceOfDodge":
                    var decreaseDodgeChanceCommand = new DecreaseDodgeChanceCommand(Convert(parameters[0]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseDodgeChanceCommand),
                        () => _composite.Unperform(decreaseDodgeChanceCommand));
                
                case "DealFixedDamage":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.FixedDamage((int) parameters[0]),
                        () => _composite.Perform(null));
                case "DealPercentDamage":
                    var commandDamage = new PercentDamageCommand((int) parameters[0], (string) parameters[1]);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandDamage),
                        () => _composite.Perform(null));
                case "FixedHealing":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.FixedHeal((int) parameters[0]),
                        () => _composite.Perform(null));
                case "PercentHealing":
                    var commandHeal = new PercentHealCommand((int) parameters[0]);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandHeal),
                        () => _composite.Perform(null));
                case "IncreaseAggression":
                    var increaseAggressionCommand = new IncreaseAggressionCommand(Convert(parameters[0]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(increaseAggressionCommand),
                        () => _composite.Unperform(increaseAggressionCommand));
                case "DecreaseAggression":
                    var decreaseAggressionCommand = new DecreaseAggressionCommand(Convert(parameters[0]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(decreaseAggressionCommand),
                        () => _composite.Unperform(decreaseAggressionCommand));
                case "RecalculatePriorities":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.RecalculatePriorities(),
                        () => _composite.Perform(null));
                case "DecreaseFixedEnergy":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.DecreaseFixedEnergy((int) parameters[0]),
                        () => _composite.Perform(null));
                case "IncreaseFixedEnergy":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.IncreaseFixedEnergy((int) parameters[0]),
                        () => _composite.Perform(null));
                case "IncreasePercentEnergy":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.IncreasePercentEnergy((int) parameters[0]),
                        () => _composite.Perform(null));
                case "DecreasePercentEnergy":
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.DecreasePercentEnergy((int) parameters[0], (string) parameters[1]),
                        () => _composite.Perform(null));
                case "FixedLifesteal":
                    var fixedLifestealDamage = new FixedLifestealCommand
                        ((int) parameters[0], (IMinion) parameters[1]);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(fixedLifestealDamage),
                        () => _composite.Perform(null));
                case "PercentLifesteal":
                    var percentLifestealDamage = new PercentLifestealCommand
                        ((int) parameters[0], (string) parameters[1], (IMinion) parameters[2]);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(percentLifestealDamage),
                        () => _composite.Perform(null));
                case "Stun":
                    var stunCommand = new StunCommand();
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(stunCommand),
                        () => _composite.Unperform(stunCommand));
                case "Silence":
                    var silenceCommand = new SilenceCommand();
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(silenceCommand),
                        () => _composite.Unperform(silenceCommand));
                case "Immortality":
                    var immortalityCommand = new ImmortalityCommand();
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(immortalityCommand),
                        () => _composite.Unperform(immortalityCommand));
                case "FixedShield":
                    var commandFixedShield = new FixedShieldCommand((int) parameters[0], commandClass);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandFixedShield),
                        () => _composite.Unperform(commandFixedShield));
                case "PercentShield":
                    var commandPercentShield = 
                        new PercentShieldCommand((int) parameters[0], (string) parameters[1], commandClass);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandPercentShield),
                        () => _composite.Unperform(commandPercentShield));
                case "DamageReflection":
                    var damageReflectionShield = new DamageReflectionCommand((int) parameters[0]);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(damageReflectionShield),
                        () => _composite.Unperform(damageReflectionShield));
                case "Summon":
                    var commandSummonCommand = new SummonCommand((string) parameters[0], (int) parameters[1], 
                        _minionFactory, _characters, _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () =>
                        {
                            commandSummonCommand.Perform(null);
                            return _composite.Perform(null);
                        },
                        () => {
                            commandSummonCommand.Undo(null);
                            return _composite.Perform(null);
                        });
                case "FixedDOT":
                    var commandFixedDot = new FixedDOTCommand((int) parameters[0], Convert(parameters[1]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandFixedDot),
                        () => _composite.Unperform(commandFixedDot));
                case "PercentDOT":
                    var commandPercentDot = new PercentDOTCommand((int) parameters[0], (int) parameters[2], _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandPercentDot),
                        () => _composite.Unperform(commandPercentDot));
                case "FixedHOT":
                    var commandFixedHot = new FixedHOTCommand((int) parameters[0], Convert(parameters[1]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandFixedHot),
                        () => _composite.Unperform(commandFixedHot));
                case "PercentHOT":
                    var commandPercentHot = new PercentHOTCommand((int) parameters[0], Convert(parameters[1]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandPercentHot),
                        () => _composite.Unperform(commandPercentHot));
                case "FixedEOT":
                    var commandFixedEot = new FixedEOTCommand((int) parameters[0], Convert(parameters[1]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandFixedEot),
                        () => _composite.Unperform(commandFixedEot));
                case "PercentEOT":
                    var commandPercentEot = new PercentEOTCommand((int) parameters[0], Convert(parameters[1]), _effectService);
                    return new KeyValuePair<Func<Result>, Func<Result>>(
                        () => _composite.Perform(commandPercentEot),
                        () => _composite.Unperform(commandPercentEot));
                default:
                    Debug.Log($"Can't create function '{action}' with parameters {Json.Serialize(parameters)}");
                    return new KeyValuePair<Func<Result>, Func<Result>>((() => Result.Error), (() => Result.Error));
            }
        }

        private float Convert(object obj)
        {
            return obj as int? ?? (float) obj;
        }
    }

    public interface IMinionCommand
    {
        public void Perform(IMinion minion);
        public void Undo(IMinion minion);
    }
}