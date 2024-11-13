using Facebook.Unity;
using GameAnalyticsSDK;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Installers;
using Realization.GameStateMachine.Interfaces;
using UnityEngine;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private TutorialService _tutorialService;

        public BootstrapState(IGameStateMachine gameStateMachine, IStaticDataService staticDataService,SceneLoader sceneLoader,LoadingCurtain loadingCurtain, TutorialService tutorialService)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _tutorialService = tutorialService;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            InitializeServices();
            _gameStateMachine.Enter<LoadProgressState>();

        }
        private void InitializeServices()
        {
            _staticDataService.Load();
            GameAnalytics.Initialize();
            FB.Init(InitCallback);
            _tutorialService.Init();
            _tutorialService.Start();
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                // Signal an app activation App Event
                FB.ActivateApp();
                // Continue with Facebook SDK
                // ...
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }

        public void Exit()
        {
        }

        public class Factory : PlaceholderFactory<IGameStateMachine, BootstrapState>
        {
        }
    }
}