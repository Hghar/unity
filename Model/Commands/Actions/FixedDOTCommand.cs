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
    public class FixedDOTCommand : IMinionCommand
    {
        private int _value;
        private float _frequency;
        private CancellationTokenSource _cancellationToken = new();
        private IVisualEffectService _effectService;

        public FixedDOTCommand(int value, float frequency, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _frequency = frequency;
            _value = value;
        }

        public async void Perform(IMinion minion)
        {
            while (_cancellationToken.IsCancellationRequested == false)
            {
                if(minion == null || _cancellationToken.IsCancellationRequested || minion.GameObject == null)
                    return;
                
                _effectService.Create(VisualEffectType.Damage, minion);
                minion.Damage(new Damage(_value));
                await Task.Delay(TimeSpan.FromSeconds(_frequency), cancellationToken: _cancellationToken.Token);
            }
        }

        public void Undo(IMinion minion)
        {
            _cancellationToken.Cancel();
        }
    }
}