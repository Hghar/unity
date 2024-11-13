using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Fight.Damaging;
using Model.Commands.Creation;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class PercentDOTCommand : IMinionCommand
    {
        private int _value;
        private float _frequency;
        private readonly IVisualEffectService _effectService;
        private CancellationTokenSource _cancellationToken = new();

        public PercentDOTCommand(int value, float frequency, IVisualEffectService effectService)
        {
            _frequency = frequency;
            _effectService = effectService;
            _value = value;
        }

        public async void Perform(IMinion minion)
        {
            while (_cancellationToken.IsCancellationRequested == false)
            {
                if(minion == null || _cancellationToken.IsCancellationRequested)
                    return;
                
                _effectService.Create(VisualEffectType.Damage, minion);
                minion.Damage(new Damage(minion.Parameters.Health.MaxValue * (_value * 0.01f)));
                await Task.Delay(TimeSpan.FromSeconds(_frequency), cancellationToken: _cancellationToken.Token);
            }
        }

        public void Undo(IMinion minion)
        {
            _cancellationToken.Cancel();
        }
    }
}