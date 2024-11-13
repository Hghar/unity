using System.Collections.Generic;
using Model.Commands.Parts;
using Model.Commands.Types;
using Units;

namespace Model.Commands
{
    public class CommandUpdater
    {
        private Dictionary<KeyValuePair<IMinion, CommandClass>, ICommand> _commands = new();

        public void AddCommand(ICommand command, IMinion caster, CommandClass commandClass)
        {
            
        }

        public void Has(IMinion caster, CommandClass commandClass)
        {
            
        }
    }

    public class UpdatableCommand : ICommand
    {
        private bool _performed;
        private bool _updated;
        
        private IUpdatableCommand _workingCommand;
        private IUpdatableCommand _newCommand;
        public float Duration { get; }

        public UpdatableCommand(IUpdatableCommand newCommand, IUpdatableCommand workingCommand, float duration)
        {
            _newCommand = newCommand;
            _workingCommand = workingCommand;
            Duration = duration;
        }
        
        public void Perform()
        {
            if(_performed || _updated)
                return;
            
            if(_workingCommand.Performed)
            {
                _newCommand.Perform();
                _performed = true;
                return;
            }

            _workingCommand.UpdateDuration(Duration);
            _updated = true;
        }

        public void Undo()
        {
            if(_performed)
                _newCommand.Undo();
        }
    }
}