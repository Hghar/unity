using System;
using System.Linq;
using GameAnalyticsSDK;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.WindowService;
using Model.Economy;
using Realization.GameStateMachine.Interfaces;
using Realization.General;
using UnityEngine;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class WinState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStorage _storage;
        private readonly IWindowService _windowService;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;

        private int LastPassedStage
        {
            get => _storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber;
            set => _storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber = value;
        }

        private Biom SelectedBiom => _storage.PlayerProgress.Bioms.SelectedBiom;

        public WinState(IGameStateMachine gameStateMachine, IStorage storage, IWindowService windowService,
                ISaveLoadService saveLoadService, IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _storage = storage;
            _windowService = windowService;
            _saveLoadService = saveLoadService;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _windowService.Open(WindowId.WinScreen);
            SetAnalyticsEvent();
            IncreaseCompletedStages();


            SelectedBiom.Shop.Level = 0;
            SelectedBiom.CurrentRoom = 0;
            if (_staticDataService.ForBioms(SelectedBiom.Key).Configs.Count > SelectedBiom.CompletedStagesCount)
            {
                LastPassedStage++;
            }
            else
            {
                if (_staticDataService.ForBioms(SelectedBiom.Key).Configs.Count >= SelectedBiom.CompletedStagesCount)
                {
                    LastPassedStage++;
                }

                OpenNextBiom();
            }

            _saveLoadService.Save();
        }

        private void SetAnalyticsEvent()
        {
            string data =
                    $" Seconds {(DateTime.Now - _storage.PlayerProgress.Analytics.LevelStartTime).Seconds} " + //Time spent on level before completion (in seconds): 
                    $" SpentDays {(int)_storage.PlayerProgress.Analytics.SpentDays}"; //Total number of days since registration: 

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,
                    $" Biom {SelectedBiom.Key} Level {LastPassedStage}",
                    data);
        }

        private void IncreaseCompletedStages()
        {
            if (_storage.PlayerProgress.Bioms.SelectedBiom.CompletedStagesCount < _staticDataService
                        .ForBioms(_storage.PlayerProgress.Bioms.SelectedBiom.Key).Configs.Count)
            {
                _storage.PlayerProgress.Bioms.SelectedBiom.CompletedStagesCount++;
            }
        }

        public void Exit()
        {
        }

        private void OpenNextBiom()
        {
            BiomeData biomeData = _staticDataService.ForBioms(SelectedBiom.Key);
            if (biomeData.Configs.Count > _storage.PlayerProgress.Bioms.SelectedBiom.CompletedStagesCount)
            {
                var first = _storage.PlayerProgress.Bioms.Opened.First(x =>
                        x.Key == SelectedBiom.Key);
                first.LastPassedStageNumber++;
            }

            Biom lastOpenedBiom = _storage.PlayerProgress.Bioms.Opened.OrderBy(x => x.Key).First();
            if (_storage.PlayerProgress.Bioms.Opened.Count(x => x.Key == lastOpenedBiom.Key + 1) > 0)
                return;

            if (_staticDataService.ForBioms(lastOpenedBiom.Key + 1) == null)
            {
                Debug.LogError("In config haven't next biom");
                return;
            }

            Biom biom = new Biom();
            biom.LastPassedStageNumber = 1;
            biom.CurrentRoom = 0;
            biom.Shop = new ShopData();
            biom.Shop.Level = 0;
            biom.Key = lastOpenedBiom.Key + 1;

            _storage.PlayerProgress.Bioms.Opened.Add(biom);
        }

        public class Factory : PlaceholderFactory<IGameStateMachine, WinState>
        {
        }
    }
}