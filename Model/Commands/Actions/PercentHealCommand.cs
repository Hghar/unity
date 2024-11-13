using Fight.Damaging;
using Model.Commands.Creation;
using Units;

namespace Model.Commands.Actions
{
    public class PercentHealCommand : IMinionCommand
    {
        private int _percents;
        private int _value;

        public PercentHealCommand(int percents)
        {
            _percents = percents;
        }

        public void Perform(IMinion minion)
        {
            _value = minion.PercentHeal(_percents);
        }

        public void Undo(IMinion minion)
        {
            minion.Damage(new Damage(_value));
        }
    }
}