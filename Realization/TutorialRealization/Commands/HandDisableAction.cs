using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Model.Economy;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using Units;

namespace Realization.TutorialRealization.Commands
{
    public class HandDisableAction : IAction
    {
        private TutorialHand _hand;

        public HandDisableAction(TutorialHand hand)
        {
            _hand = hand;
        }

        public UniTask Perform()
        {
            _hand.Disable();
            return UniTask.CompletedTask;
        }
    }
    
    public class SetCurrencyAction : IAction
    {
        private IStorage _storage;
        private Currency _currency;
        private int _value;

        public SetCurrencyAction(IStorage storage, Currency currency, int value)
        {
            _value = value;
            _currency = currency;
            _storage = storage;
        }

        public UniTask Perform()
        {
            _storage.SetResource(_currency, _value);
            return UniTask.CompletedTask;
        }
    }
    
    public class SetGeneralUpgradeAction : IAction
    {
        private IStorage _storage;
        private int _value;

        public SetGeneralUpgradeAction(IStorage storage, int value)
        {
            _value = value;
            _storage = storage;
        }

        public UniTask Perform()
        {
            _storage.PlayerProgress.CoreUpgrades.CurrentGeneralLevel = _value;
            return UniTask.CompletedTask;
        }
    }
    
    public class SetClassUpgradeAction : IAction
    {
        private IStorage _storage;
        private int _value;
        private ClassParent _classParent;

        public SetClassUpgradeAction(IStorage storage, ClassParent classParent, int value)
        {
            _classParent = classParent;
            _value = value;
            _storage = storage;
        }

        public UniTask Perform()
        {
            _storage.PlayerProgress.CoreUpgrades.Stats.SetLevel(_classParent, _value);
            return UniTask.CompletedTask;
        }
    }
}