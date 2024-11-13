using Model.Commands.Types;
using Units;

namespace Model.Commands.Actions
{
    public class SilenceCommand : UndoableCommand<int>
    {
        protected override int PerformInternal(IMinion minion)
        {
            minion.Silence();
            return 0;
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.Unsilence();
        }
    }
}