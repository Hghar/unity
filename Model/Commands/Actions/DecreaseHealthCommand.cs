using Model.Commands.Types;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class DecreaseHealthCommand : UndoableCommand<int>
    {
        private readonly int _percents;
        private IVisualEffectService _effectService;

        public DecreaseHealthCommand(int percents, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _percents = percents;
        }

        protected override int PerformInternal(IMinion minion)
        {
            _effectService.Create(VisualEffectType.Debuff, minion);
            var value = (int) Mathf.Round(minion.Parameters.Health.StartValue * (_percents * 0.01f));
            return minion.DecreaseHealth(value);
        }

        protected override void UndoInternal(IMinion minion, int value)
        {
            minion.IncreaseHealth(value);
        }
    }
}