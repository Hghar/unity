using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class IncreaseDodgeChanceCommand : UndoableCommand<float>
    {
        private readonly float _value;
        private IVisualEffectService _effectService;

        public IncreaseDodgeChanceCommand(float value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            return minion.IncreaseDodgeChance(_value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.DecreaseDodgeChance(value);
        }
    }
}