using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class DecreaseCriticalChanceCommand : UndoableCommand<float>
    {
        private readonly float _value;
        private IVisualEffectService _effectService;

        public DecreaseCriticalChanceCommand(float value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Debuff, minion);
            return minion.DecreaseCriticalChance(_value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.IncreaseCriticalChance(value);
        }
    }
}