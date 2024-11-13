using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class IncreaseAggressionCommand : UndoableCommand<float>
    {
        private readonly float _value;
        private IVisualEffectService _effectService;

        public IncreaseAggressionCommand(float value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.AggressionUp, minion);
            return minion.IncreaseAggression(_value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.DecreaseAggression(value);
        }
    }
}