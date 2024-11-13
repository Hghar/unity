using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Battle;
using Cysharp.Threading.Tasks;
using Model.Commands.Types;
using UnityEngine;

namespace Model.Commands.Helpers
{
    public class CommandWorker : IDisposable
    {
        private readonly Dictionary<PassiveCommand, CancellationTokenSource> _passiveWorkers = new();
        private readonly Dictionary<PassiveCommand,Action> _performs = new();
        private readonly Dictionary<PassiveCommand,Action> _cancels = new();
        private readonly IBattleFinishPublisher _finishPublisher;

        private void StopWorkers()
        {
            foreach (var worker in _passiveWorkers)
            {
                CancelWorking(worker.Key);
                Unsubscribe(worker.Key);
            }
            _passiveWorkers.Clear();
        }

        public void Prepare(PassiveCommand passiveCommand)
        {
            _passiveWorkers.Add(passiveCommand, new CancellationTokenSource());
            Action perform = () => { Work(passiveCommand); };
            Action cancel = () => { Cancel(passiveCommand); };
            _performs.Add(passiveCommand, perform);
            _cancels.Add(passiveCommand, cancel);
            passiveCommand.Performed += perform;
            passiveCommand.Canceled += cancel;
        }

        public void Dispose()
        {
            foreach (var worker in _passiveWorkers)
            {
                _passiveWorkers[worker.Key].Cancel();
                Unsubscribe(worker.Key);
            }
            _passiveWorkers.Clear();
        }

        private void Cancel(PassiveCommand passiveCommand)
        {
            CancelWorking(passiveCommand);
            Unsubscribe(passiveCommand);
            _cancels.Remove(passiveCommand);
            _performs.Remove(passiveCommand);
        }

        private void CancelWorking(PassiveCommand passiveCommand)
        {
            _passiveWorkers[passiveCommand].Cancel();
            _passiveWorkers.Remove(passiveCommand);
        }

        private void Unsubscribe(PassiveCommand passiveCommand)
        {
            passiveCommand.Performed -= _performs[passiveCommand];
            passiveCommand.Canceled -= _cancels[passiveCommand];
        }

        private async void Work(PassiveCommand passiveCommand)
        {
            var endTime = Time.time + passiveCommand.Duration;
            while (Time.time < endTime)
            {
                await Task.Delay(TimeSpan.FromSeconds(passiveCommand.Frequency));
                
                passiveCommand.PerformCommand();

                if (_passiveWorkers.ContainsKey(passiveCommand) == false || _passiveWorkers[passiveCommand].IsCancellationRequested)
                {
                    return;
                }
            }
        }
    }
}