using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class IncreaseCriticalChanceCommand : UndoableCommand<float>
    {
        private readonly float _value;
        private IVisualEffectService _effectService;

        public IncreaseCriticalChanceCommand(float value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            return minion.IncreaseCriticalChance(_value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.DecreaseCriticalChance(value);
        }
    }
}