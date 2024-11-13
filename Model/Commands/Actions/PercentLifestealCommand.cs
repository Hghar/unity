using Model.Commands.Creation;
using Units;

namespace Model.Commands.Actions
{
    public class PercentLifestealCommand : IMinionCommand
    {
        private int _percents;
        private int _value;
        private IMinion _caster;
        private string _healthTarget;

        public PercentLifestealCommand(int percents, string healthTarget, IMinion caster)
        {
            _healthTarget = healthTarget;
            _caster = caster;
            _percents = percents;
        }

        public void Perform(IMinion minion)
        {
            minion.PercentLifesteal(_percents, _healthTarget, _caster);
        }

        public void Undo(IMinion minion)
        {
            
        }
    }
}