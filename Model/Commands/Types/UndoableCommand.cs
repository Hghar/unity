using System.Collections.Generic;
using Model.Commands.Creation;
using Units;

namespace Model.Commands.Types
{
    public abstract class UndoableCommand<T> : IMinionCommand
    {
        private Dictionary<IMinion, T> _values = new();
        private bool _undone;

        public void Perform(IMinion minion)
        {
            var value = PerformInternal(minion);
            _values.Add(minion, value);
        }

        public void Undo(IMinion minion)
        {
            if(_undone)
                return;
            
            foreach (var value in _values)
            {
                UndoInternal(value.Key, value.Value);
            }

            _undone = true;
        }

        protected abstract T PerformInternal(IMinion minion);

        protected abstract void UndoInternal(IMinion minion, T value);
    }
}