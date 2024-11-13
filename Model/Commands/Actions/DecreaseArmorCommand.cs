using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class DecreaseArmorCommand : UndoableCommand<int>
    {
        private readonly int _value;
        private IVisualEffectService _effectService;

        public DecreaseArmorCommand(int value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override int PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Debuff, minion);
            return minion.DecreaseArmor(_value);
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.IncreaseArmor(value);
        }
    }
}