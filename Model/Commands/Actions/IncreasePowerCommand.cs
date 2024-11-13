using Model.Commands.Types;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class IncreasePowerCommand : UndoableCommand<int>
    {
        private readonly int _percents;
        private IVisualEffectService _effectService;

        public IncreasePowerCommand(int percents, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _percents = percents;
        }

        protected override int PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            var value = (int) Mathf.Round(minion.Parameters.Damage.StartValue * (_percents * 0.01f));
            return minion.IncreasePower(value);
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.DecreasePower(value);
        }
    }
}