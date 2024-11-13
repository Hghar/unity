using System;
using GameAnalyticsSDK;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService;
using Model.Economy;
using Realization.GameStateMachine.Interfaces;
using Realization.General;
using Realization.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Realization.GameStateMachine.States
{
    public class LoseState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IWindowService _windowService;
        private readonly IStorage _storage;
        private MenuButton _menuButton;

        public LoseState(IGameStateMachine gameStateMachine, IWindowService windowService,IStorage storage)
        {
            _gameStateMachine = gameStateMachine;
            _windowService = windowService;
            _storage = storage;
        }

        public void Enter()
        {
            _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level = 0;
            _storage.PlayerProgress.Bioms.SelectedBiom.CurrentRoom = 0;
            _windowService.Open(WindowId.LosePanel);
            SetAnalyticsEvent();
        }

        public void Exit()
        {
        }
        private void SetAnalyticsEvent()
        {
            string data = $" Seconds {( DateTime.Now-_storage.PlayerProgress.Analytics.LevelStartTime).Seconds} "+ //Time spent on level before completion (in seconds): 
                          $" SpentDays {(int)_storage.PlayerProgress.Analytics.SpentDays}"; //Total number of days since registration: 

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail,
            $" Biom {_storage.PlayerProgress.Bioms.SelectedBiom.Key} Level {_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber}",
            " Player is dead",data);
        }
        public class Factory : PlaceholderFactory<IGameStateMachine, LoseState>
        {
        }
    }
}