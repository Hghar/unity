using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class IncreaseTimeBetweenAttacksCommand : UndoableCommand<float>
    {
        private readonly int _percents;
        private IVisualEffectService _effectService;

        public IncreaseTimeBetweenAttacksCommand(int percents, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _percents = percents;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            var value = (minion.Parameters.Cooldown.StartValue * (_percents * 0.01f));
            return minion.IncreaseTimeBetweenAttacks(value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.DecreaseTimeBetweenAttacks(value);
        }
    }
}