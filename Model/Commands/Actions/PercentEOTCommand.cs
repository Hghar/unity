using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model.Commands.Creation;
using Realization.VisualEffects;
using Units;
using UnityEngine;

namespace Model.Commands.Actions
{
    public class PercentEOTCommand : IMinionCommand
    {
        private int _value;
        private float _frequency;
        private CancellationTokenSource _cancellationToken = new();
        private IVisualEffectService _effectService;

        public PercentEOTCommand(int value, float frequency, IVisualEffectService effectService)
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

                _effectService.Create(VisualEffectType.Energy, minion);
                minion.AddEnergy((int)(minion.Parameters.Energy.MaxValue * (_value * 0.01f)));

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_frequency), cancellationToken: _cancellationToken.Token);
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(_frequency));
                    Debug.LogError("cancellationToken: _cancellationToken.Token is null");
                }
            }
        }

        public void Undo(IMinion minion)
        {
            _cancellationToken.Cancel();
        }
    }
}