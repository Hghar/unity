using System;

namespace Model.Commands.Types
{
    public class PassiveCommand : ICommand
    {
        private ActiveCommand _command;
        public event Action Performed;
        public event Action Canceled;
        
        public PassiveCommand(ActiveCommand command, float frequency, float duration = float.MaxValue)
        {
            _command = command;
            Duration = duration;
            Frequency = frequency;
        }

        public float Duration { get; }
        public float Frequency { get; }

        public void Perform()
        {
            Performed?.Invoke();
        }

        public void Undo()
        {
            Canceled?.Invoke();
        }

        public void PerformCommand()
        {
            _command.Perform();
        }

        public void UndoCommand()
        {
            _command.Undo();
        }
    }
}