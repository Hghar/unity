using Model.Commands.Types;
using Units;

namespace Model.Commands.Actions
{
    public class ImmortalityCommand : UndoableCommand<int>
    {
        protected override int PerformInternal(IMinion minion)
        {
            minion.Immortality();
            return 0;
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.Unimmortal();
        }
    }
}