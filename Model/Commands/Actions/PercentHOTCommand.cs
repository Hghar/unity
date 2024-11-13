using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model.Commands.Creation;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Actions
{
    public class PercentHOTCommand : IMinionCommand
    {
        private int _value;
        private float _frequency;
        private CancellationTokenSource _cancellationToken = new();
        private IVisualEffectService _effectService;

        public PercentHOTCommand(int value, float frequency, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _frequency = frequency;
            _value = value;
        }

        public async void Perform(IMinion minion)
        {
            while (_cancellationToken.IsCancellationRequested == false)
            {
                if(minion == null || _cancellationToken.IsCancellationRequested)
                    return;
                
                minion.Parameters.Health.Increase((int)(minion.Parameters.Health.MaxValue * (_value * 0.01f)));
                _effectService.Create(VisualEffectType.Heal, minion);
                await Task.Delay(TimeSpan.FromSeconds(_frequency), cancellationToken: _cancellationToken.Token);
            }
        }

        public void Undo(IMinion minion)
        {
            _cancellationToken.Cancel();
        }
    }
}