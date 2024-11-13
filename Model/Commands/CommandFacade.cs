using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Model.Commands.Actions;
using Model.Commands.Creation;
using Model.Commands.Helpers;
using Model.Commands.Parts;
using Model.Commands.Types;
using ModestTree.Util;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands
{
    public class CommandFacade : IDisposable, ICommandFacade
    {
        private CommandBuilder _builder;
        private CommandDeactivator _deactivator;
        private CommandWorker _worker;
        private IVisualEffectService _visualEffectService;

        public CommandFacade(CommandBuilder builder, CommandDeactivator deactivator, CommandWorker worker,
            IVisualEffectService effectService)
        {
            _visualEffectService = effectService;
            _worker = worker;
            _deactivator = deactivator;
            _builder = builder;
        }

        public ICommand MakeCommand(string value, IMinion caster = null)
        {
            if (value is null or "-" or "" or " ")
                return new EmptyCommand();

            List<string> words = ParseWords(value);

            words[0] = words[0] == "SetEffect" ? CommandType.Active.ToString() : words[0];
            var typeWithParameters = SplitParameters(words[0]);
            Enum.TryParse<CommandType>(typeWithParameters.First, out var type);
            
            var actionNameWithParameters = SplitParameters(words[1]);
            
            var targetWithParameters = SplitParameters(words[2]);
            Enum.TryParse<TargetType>(targetWithParameters.First, out var target);
            List<object> targetParameters = new();
            targetParameters.AddRange(targetWithParameters.Second);
            if(caster != null)
                targetParameters.Add(caster);
            
            var nameWithParameters = SplitParameters(words[3]);
            Enum.TryParse<CommandTime>(nameWithParameters.First, out var time);

            float duration = float.MaxValue;
            if (time == CommandTime.Duration)
            {
                float timeValue = 
                    nameWithParameters.Second[0] 
                        as int? ?? (float)nameWithParameters.Second[0];
                duration = timeValue;
            }
            
            List<object> parameters = new();
            parameters.AddRange(actionNameWithParameters.Second);
            if(caster != null)
                parameters.Add(caster);

            ICommand command;
            var builderParameters = 
                new StringCommandParameters(
                    CommandType.Active,
                    actionNameWithParameters.First, parameters.ToArray(),
                    target, targetParameters.ToArray(),
                    duration,
                    caster
                );

            Action aoeEffect = null;
            if (words.Count >= 5)
            {
                var radiusWithParameters = SplitParameters(words[4]);
                builderParameters.AddRadius((int)radiusWithParameters.Second[0]);
                aoeEffect = CreateAoeEffect(builderParameters.Action, (int)radiusWithParameters.Second[0], caster);
            }
            
            if (type == CommandType.Active)
            {
                var internalCommand = _builder.BuildWithParameters(builderParameters);
                command = new Command(internalCommand, _deactivator, aoeEffect,
                    builderParameters.CommandClass, builderParameters.Caster);
            }
            else
            {
                command = 
                    new PassiveCommandDecorator(
                        builderParameters,
                        _builder,
                        _deactivator, 
                        typeWithParameters.Second[0] as int? ?? (float)nameWithParameters.Second[0],
                        aoeEffect);
            }
            return command;
        }

        private List<string> ParseWords(string value)
        {
            List<string> words = new();
            
            value = value.Replace(',', '.');
            string pattern = @"(_)(?=(?:[^\(\)]*\([^\(\)]*\))*[^\(\)]*$)";
            string[] result = Regex.Split(value, pattern);
            foreach (string word in result)
            {
                if (word == "_")
                    continue;
                words.Add(word);
            }

            return words;
        }

        private Action CreateAoeEffect(string actionName, int radius, IMinion caster)
        {
            switch (actionName)
            {
                case "DealFixedDamage":
                    return (() => _visualEffectService.CreateAOE(VisualEffectType.SplashDamage, caster, radius));
                case "DealPercentDamage":
                    return (() => _visualEffectService.CreateAOE(VisualEffectType.SplashDamage, caster, radius));
                case "FixedHealing":
                    return (() => _visualEffectService.CreateAOE(VisualEffectType.SplashHeal, caster, radius));
                case "PercentHealing":
                    return (() => _visualEffectService.CreateAOE(VisualEffectType.SplashHeal, caster, radius));
                case "FixedLifesteal":
                    return (() => _visualEffectService.CreateAOE(VisualEffectType.SplashLifesteal, caster, radius));
                case "PercentLifesteal":
                    return (() => _visualEffectService.CreateAOE(VisualEffectType.SplashLifesteal, caster, radius));
                default:
                    Debug.LogError($"There is no effect for aoe {actionName}");
                    return (() => { });
            }
        }

        private ValuePair<string, object[]> SplitParameters(string value)
        {
            var separator = value.IndexOf('(');
            string name;
            if (separator == -1)
            {
                name = value;
                return new ValuePair<string, object[]>(name, new object[]{});
            }
            
            value = value.Remove(value.Length - 1);
            name = value.Substring(0, separator);
            var parameters = value.Substring(separator+1, value.Length-separator-1);
            var splitParameters = parameters.Split(';');
            var objectParameters = new List<object>();
            
            foreach (var parameter in splitParameters)
            {
                if (int.TryParse(parameter, out int intVal))
                    objectParameters.Add(intVal);
                else if (float.TryParse(parameter, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float floatVal))
                    objectParameters.Add(floatVal);
                else
                    objectParameters.Add(parameter);
            }
            return new ValuePair<string, object[]>(name, objectParameters.ToArray());
        }

        public void Dispose()
        {
            _deactivator?.Dispose();
            _worker?.Dispose();
        }
    }
}