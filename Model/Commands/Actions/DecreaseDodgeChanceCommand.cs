using Model.Commands.Types;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class DecreaseDodgeChanceCommand : UndoableCommand<float>
    {
        private readonly float _value;
        private IVisualEffectService _effectService;

        public DecreaseDodgeChanceCommand(float value, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _value = value;
        }

        protected override float PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Debuff, minion);
            return minion.DecreaseDodgeChance(_value);
        }

        protected override void UndoInternal(IMinion minion, float value)
        {
            minion.IncreaseDodgeChance(value);
        }
    }
}