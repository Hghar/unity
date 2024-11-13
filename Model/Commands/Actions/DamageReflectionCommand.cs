using Model.Commands.Types;
using Units;

namespace Model.Commands.Actions
{
    public class DamageReflectionCommand : UndoableCommand<int>
    {
        private int _percents;

        public DamageReflectionCommand(int percents)
        {
            _percents = percents;
        }
        protected override int PerformInternal(IMinion minion)
        {
            minion.DamageReflection(_percents);
            return 0;
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.UndoDamageReflection(_percents);
        }
    }
}