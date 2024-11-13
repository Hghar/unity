using Model.Commands.Types;
using Units;

namespace Model.Commands.Actions
{
    public class StunCommand : UndoableCommand<int>
    {
        protected override int PerformInternal(IMinion minion)
        {
            minion.Stun();
            return 0;
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.Unstun();
        }
    }
}