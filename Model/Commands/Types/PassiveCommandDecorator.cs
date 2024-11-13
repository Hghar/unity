using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fight.Fractions;
using Model.Commands.Creation;
using Model.Commands.Helpers;
using Model.Commands.Parts;
using Units;
using UnityEngine;

namespace Model.Commands.Types
{
    public class PassiveCommandDecorator : ICommand
    {
        private CommandDeactivator _deactivator;
        private List<ICommand> _commands = new();
        private float _frequency;
        private CancellationTokenSource _cancellationToken = new();
        private readonly StringCommandParameters _parameters;
        private CommandBuilder _builder;
        private Action _aoeEffect;

        public float Duration => float.MaxValue;

        public PassiveCommandDecorator(StringCommandParameters parameters, CommandBuilder builder,
            CommandDeactivator deactivator, float frequency, Action aoeEffect)
        {
            _aoeEffect = aoeEffect;
            _builder = builder;
            _parameters = parameters;
            _frequency = frequency;
            _deactivator = deactivator;
            
            if (parameters.Caster != null && parameters.Caster.Fraction == Fraction.Enemies)
                parameters.Target = CommandBuilder.InvertTarget(parameters.Target);
        }

        public async void Perform()
        {
            try
            {
                while (_cancellationToken.IsCancellationRequested == false)
                {
                    await Task.Delay(TimeSpan.FromSeconds(_frequency), cancellationToken: _cancellationToken.Token);

                    if (_deactivator.HasWorking(_parameters.CommandClass, _parameters.Caster) &&
                        _parameters.CommandClass.Action.Contains("Shield") == false)
                    {
                        _deactivator.UpdateCommand(_parameters.CommandClass, _parameters.Caster);
                        _aoeEffect?.Invoke();
                        continue;
                    }
                    
                    ICommand command;
                    lock (_builder)
                    {
                        command = _builder.BuildWithParameters(_parameters, false);
                        command.Perform();
                    }
                    _aoeEffect?.Invoke();
                    _commands.Add(command);
                    _deactivator.Add(command, _parameters.CommandClass, _parameters.Caster);
                }
            }
            catch (TaskCanceledException e)
            {
                Debug.LogWarning(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Undo()
        {
            _cancellationToken.Cancel();
            foreach (var command in _commands)
            {
                command.Undo();
            }
            Debug.Log($"undo all passives");
        }
    }
}