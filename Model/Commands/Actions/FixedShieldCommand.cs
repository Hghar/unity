using Model.Commands.Parts;
using Model.Commands.Types;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class FixedShieldCommand : UndoableCommand<int>
    {
        private int _shieldValue;
        private CommandClass _class;

        public FixedShieldCommand(int shieldValue, CommandClass @class)
        {
            _class = @class;
            _shieldValue = shieldValue;
        }

        protected override int PerformInternal(IMinion minion)
        {
            minion.AddShield(_shieldValue, _class);
            return 0;
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.RemoveShield(_class);
        }
    }
}