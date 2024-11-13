using System.Runtime.CompilerServices;
using Model.Commands.Parts;
using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class PercentShieldCommand : UndoableCommand<int>
    {
        private int _value;
        private int _shieldValue;
        private string _healthTarget;
        private CommandClass _class;

        public PercentShieldCommand(int shieldValue, string healthTarget, CommandClass @class)
        {
            _class = @class;
            _healthTarget = healthTarget;
            _shieldValue = shieldValue;
        }

        protected override int PerformInternal(IMinion minion)
        {
            
            float shield;
            if(_healthTarget == "Max")
                shield = minion.Parameters.Health.MaxValue * (_shieldValue * 0.01f);
            else
                shield = minion.Parameters.Health.Value * (_shieldValue * 0.01f);
            minion.AddShield((int)shield, _class);
            return 0;
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.RemoveShield(_class);
        }
    }
}