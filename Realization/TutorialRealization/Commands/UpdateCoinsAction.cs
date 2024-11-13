using System;
using Cysharp.Threading.Tasks;
using Model.Economy;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Zenject;

namespace Realization.TutorialRealization.Commands
{
    public class UpdateCoinsAction : IAction
    {
        private int _value;
        private DiContainer _container;

        public UpdateCoinsAction(DiContainer container, int value)
        {
            _container = container;
            _value = value;
        }

        public UniTask Perform()
        {
            if(_value >= 0)
                _container.Resolve<IStorage>().AddResource(Currency.Gold, _value, false);
            else
            {
                _container.Resolve<IStorage>().SpendResource(Currency.Gold, Math.Abs(_value), false);
            }
            return UniTask.CompletedTask;
        }
    }
}