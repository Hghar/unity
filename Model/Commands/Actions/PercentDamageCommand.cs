using Model.Commands.Types;
using Units;

namespace Model.Commands.Actions
{
    public class PercentDamageCommand : UndoableCommand<float>
    {
        private int _percents;
        private string _healthTarget;

        public PercentDamageCommand(int percents, string damageTarget)
        {
            _healthTarget = damageTarget;
            _percents = percents;
        }

        protected override float PerformInternal(IMinion minion)
        {
            return minion.PercentDamage(_percents, _healthTarget);
        }
        
        protected override void UndoInternal(IMinion affectable, float value)
        {
            
        }
    }
}