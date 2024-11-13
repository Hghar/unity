using Model.Commands.Creation;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class FixedLifestealCommand : IMinionCommand
    {
        private int _percents;
        private int _value;
        private IMinion _caster;

        public FixedLifestealCommand(int percents, IMinion caster)
        {
            _caster = caster;
            _percents = percents;
        }

        public void Perform(IMinion minion)
        {
            minion.FixedLifesteal(_percents, _caster);
        }

        public void Undo(IMinion minion)
        {
            
        }
    }
}