using Model.Commands.Types;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class DecreasePowerCommand : UndoableCommand<int>
    {
        private readonly int _percents;
        private IVisualEffectService _effectService;

        public DecreasePowerCommand(int percents, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _percents = percents;
        }

        protected override int PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Debuff, minion);
            var value = (int) Mathf.Round(minion.Parameters.Damage.StartValue * (_percents * 0.01f));
            return minion.DecreasePower(value);
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.IncreasePower(value);
        }
    }
}