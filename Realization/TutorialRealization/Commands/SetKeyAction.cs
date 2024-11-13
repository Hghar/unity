using CustomInput;
using CustomInput.Picking;
using Cysharp.Threading.Tasks;
using Fight;
using Fight.Attack;
using Infrastructure.Services.SaveLoadService;
using Model.Economy;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.Installers;
using Realization.Sets;
using Realization.Shops;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Realization.TutorialRealization.Commands
{
    public class SetKeyAction : IAction
    {
        private int _value;
        private IStorage _storage;
        private ISaveLoadService _saveLoadService;

        public SetKeyAction(IStorage storage, int value, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _storage = storage;
            _value = value;
        }

        public UniTask Perform()
        {
            _storage.PlayerProgress.TutorialData.Key = _value;
            _saveLoadService.Save();
            return UniTask.CompletedTask;
        }
    }
    
    public class SetShopOnResetAction : IAction
    {
        private Character _character;
        private int _shopIndex;
        private DiContainer _container;

        public SetShopOnResetAction(DiContainer container, Character character, int shopIndex)
        {
            _container = container;
            _shopIndex = shopIndex;
            _character = character;
        }

        public UniTask Perform()
        {
            ShopInstaller.Shop.AddToQueue(_shopIndex, _character);
            return UniTask.CompletedTask;
        }
    }
    
    public class HardUnitTapAction : IAction
    {
        private DelayedObject _minion;
        private bool _activate;

        public HardUnitTapAction(DelayedObject minion, bool activate)
        {
            _activate = activate;
            _minion = minion;
        }

        public async UniTask Perform()
        {
            var minion = await _minion.GetAsync();
            var clickables = minion.GetComponentsInChildren<IPointerClickHandler>();
            foreach (var clickable in clickables)
            {
                (clickable as MonoBehaviour).enabled = true;  
            }
            minion.GetComponentInChildren<Pickable>().Working = _activate;
        }
    }
    
    public class HardUnitHoldAction : IAction
    {
        private DelayedObject _minion;
        private bool _activate;

        public HardUnitHoldAction(DelayedObject minion, bool activate)
        {
            _activate = activate;
            _minion = minion;
        }

        public async UniTask Perform()
        {
            var minion = await _minion.GetAsync();
            var clickables = minion.GetComponentsInChildren<IPointerClickHandler>();
            foreach (var clickable in clickables)
            {
                (clickable as MonoBehaviour).enabled = true;  
            }
            minion.GetComponent<Draggable>().Working = _activate;
        }
    }
    
    public class FeatureAction : IAction
    {
        private string _featureId;
        private bool _activate;

        public FeatureAction(string featureId, bool activate)
        {
            _activate = activate;
            _featureId = featureId;
        }

        public UniTask Perform()
        {
            switch (_featureId)
            {
                case "Energy":
                    EnergyStorage.Working = _activate;
                    break;
                case "Skills":
                    CommandInvoker.WorkingFeature = _activate;
                    break;
                case "Levels":
                    Level.WorkingFeature = _activate;
                    break;
                case "SetEffects":
                    SetEffectInvoker.WorkingFeature = _activate;
                    break;
            }
            return UniTask.CompletedTask;
        }
    }
    
    public class EnableObjectAction : IAction
    {
        private DelayedObject _target;
        private bool _activate;

        public EnableObjectAction(DelayedObject target, bool activate)
        {
            _activate = activate;
            _target = target;
        }

        public async UniTask Perform()
        {
            var minion = await _target.GetAsync();
            minion.SetActive(_activate);
        }
    }
    
    public class SaveTutorialAction : IAction
    {
        private IStorage _storage;
        private ISaveLoadService _saveLoadService;

        public SaveTutorialAction(IStorage storage, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _storage = storage;
        }

        public UniTask Perform()
        {
            _storage.PlayerProgress.TutorialData.Started = true;
            _saveLoadService.Save();
            return UniTask.CompletedTask;
        }
    }
    
    public class SetRewardAction : IAction
    {
        private DelayedObject _minion;
        private int _reward;

        public SetRewardAction(DelayedObject minion, int reward)
        {
            _reward = reward;
            _minion = minion;
        }

        public async UniTask Perform()
        {
            var minion = await _minion.GetAsync();
            minion.GetComponent<DeathReward>().OverrideReward(_reward);
        }
    }
}