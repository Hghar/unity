using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;

namespace Realization.TutorialRealization.Commands
{
    public class DelayAction : IAction
    {
        private float _delay;

        public DelayAction(float delay)
        {
            _delay = delay;
        }
        
        public async UniTask Perform()
        {
            await Task.Delay(TimeSpan.FromSeconds(_delay));
        }
    }
}