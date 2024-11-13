using System;
using Model.Commands.Helpers;
using Model.Commands.Parts;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Types
{
    public class Command : ICommand
    {
        private CommandDeactivator _deactivator;
        private readonly Action _aoeEffect;
        private ICommand _command;
        private CommandClass _commandClass;
        private IMinion _caster;
        private bool _performed;

        public float Duration => _command.Duration;

        public Command(ICommand command, CommandDeactivator deactivator, Action aoeEffect,
            CommandClass commandClass, IMinion caster = null)
        {
            _caster = caster;
            _commandClass = commandClass;
            _command = command;
            _deactivator = deactivator;
            _aoeEffect = aoeEffect;
        }

        public void Perform()
        {
            if (_deactivator.HasWorking(_commandClass, _caster) &&
                _commandClass.Action.Contains("Shield") == false)
            {
                _deactivator.UpdateCommand(_commandClass, _caster);
                _aoeEffect?.Invoke();
                return;
            }
            
            _command.Perform();
            _deactivator.Add(_command, _commandClass, _caster);
            _aoeEffect?.Invoke();
            _performed = true;
        }

        public void Undo()
        {
            if(_performed)
                _command.Undo();
        }
    }
    
    public class VisualCommand : ICommand
    {
        private ICommand _command;
        private IVisualEffectService _effectService;
        private IMinion _target;

        public float Duration => _command.Duration;

        public VisualCommand(ICommand command, IVisualEffectService effectService, IMinion target)
        {
            _target = target;
            _effectService = effectService;
            _command = command;
        }

        public void Perform()
        {
            _effectService.Create(VisualEffectType.Buff, _target);
            _command.Perform();
        }

        public void Undo()
        {
            _command.Undo();
        }
    }
}