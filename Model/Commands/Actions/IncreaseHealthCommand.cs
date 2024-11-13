using Model.Commands.Types;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class IncreaseHealthCommand : UndoableCommand<int>
    {
        private readonly int _percents;
        private readonly IVisualEffectService _effectService;

        public IncreaseHealthCommand(int percents, IVisualEffectService effectService)
        {
            _percents = percents;
            _effectService = effectService;
        }

        protected override int PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Buff, minion);
            var value = (int)Mathf.Round((minion.Parameters.Health.StartValue * (_percents * 0.01f)));
            return minion.IncreaseHealth(value);
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.DecreaseHealth(value);
        }
    }
}