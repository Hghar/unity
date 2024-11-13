using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class DecreaseAggressionCommand : UndoableCommand<float>
    {
        private readonly float _value;
        private IVisualEffectService _effectService;

        public DecreaseAggressionCommand(float value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.AggressionDown, minion);
            return minion.DecreaseAggression(_value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.IncreaseAggression(value);
        }
    }
}