using System;
using System.Linq;
using GameAnalyticsSDK;
using Infrastructure.Helpers;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService;
using Infrastructure.Services.WindowService.MVVM;
using Installers;
using Model.Economy;
using Model.Economy.Resources;
using Realization.Configs;
using Realization.Economy;
using Realization.GameStateMachine.Interfaces;
using Realization.UI;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class LoadLevelState : IPaylodedState<string>
    {
        private GamePanel _gamePanel;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIViewFactory _iuiViewFactory;
        private readonly IHudFactory _hudFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IStorage _storage;
        private readonly IStaticDataService _staticDataService;
        private readonly INotificationService _notificationService;
        private readonly IUIViewFactory _viewFactory;
        private readonly IWindowService _windowService;

        public LoadLevelState(IGameStateMachine gameStateMachine,
                IUIViewFactory iuiViewFactory,
                IHudFactory hudFactory,
                SceneLoader sceneLoader,
                LoadingCurtain loadingCurtain,
                IStorage storage,
                IStaticDataService staticDataService,
                INotificationService notificationService,
                IUIViewFactory viewFactory,IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _iuiViewFactory = iuiViewFactory;
            _hudFactory = hudFactory;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _storage = storage;
            _staticDataService = staticDataService;
            _notificationService = notificationService;
            _viewFactory = viewFactory;
            _windowService = windowService;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName, onLoaded: OnLoaded);
        }

        private void OnLoaded()
        {
            ResetMoneyValue();
            ResetShopLevel();
            _iuiViewFactory.CreateUIRoot();
            _windowService.InitCore();
            HudFacade hudFacade = _hudFactory.CreateHud();
            hudFacade.EconomyBehaviour.Initialize();
            _notificationService.CreateRoot();
            _loadingCurtain.Hide();
            StartGame();
            SceneObjectPool.Instance.AddScene();
        }

        private void ResetShopLevel()
        {
            _storage.PlayerProgress.Bioms.SelectedBiom.Shop.Level = 0;
        }

        private void ResetMoneyValue()
        {
            Constants constants = _staticDataService.CharacterConfig().Constants;
            CurrencyData currency = _storage.PlayerProgress.WorldData.CurrencyData;
            currency.Pay(Currency.Gold,currency.GoldValue); //ResetGold
            currency.Add(Currency.Gold,constants.TavernBasicAmountOfGold);
        }

        private void StartGame()
        {
            /*
            string data = $"Level {_storage.PlayerProgress.Bioms.SelectedBiom.Key}" +
                          $"Total number of days since registration: {_storage.PlayerProgress.Analytics.SpentDays}";*/
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,
                    $" Biom {_storage.PlayerProgress.Bioms.SelectedBiom.Key} Level {_storage.PlayerProgress.Bioms.SelectedBiom.LastPassedStageNumber}",
                    $" SpentDays {(int)_storage.PlayerProgress.Analytics.SpentDays}");
            _storage.PlayerProgress.Analytics.LevelStartTime = DateTime.Now;
            

            _gameStateMachine.Enter<GameloopState>();
        }

        public async void Exit()
        {
            //todo fix null exception

            if (_gamePanel == null)
                return;

            await _gamePanel.ShowOff();
            _gamePanel.Disable();
        }

        public class Factory : PlaceholderFactory<IGameStateMachine, LoadLevelState>
        {
        }
    }
}