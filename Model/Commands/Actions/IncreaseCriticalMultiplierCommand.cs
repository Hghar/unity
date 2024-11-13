using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class IncreaseCriticalMultiplierCommand : UndoableCommand<float>
    {
        private readonly int _percents;
        private IVisualEffectService _effectService;

        public IncreaseCriticalMultiplierCommand(int percents, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _percents = percents;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            var value = (minion.Parameters.CriticalDamageMultiplier.StartValue * (_percents * 0.01f));
            return minion.IncreaseCriticalMultiplier(value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.DecreaseCriticalMultiplier(value);
        }
    }
}