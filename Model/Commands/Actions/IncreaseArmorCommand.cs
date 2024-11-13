using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class IncreaseArmorCommand : UndoableCommand<int>
    {
        private readonly int _value;
        private IVisualEffectService _effectService;

        public IncreaseArmorCommand(int value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override int PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            return minion.IncreaseArmor(_value);
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.DecreaseArmor(value);
        }
    }
}