using DefaultNamespace;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.SceneLoader;
using Installers;
using Realization.GameStateMachine.Interfaces;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class RestartState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;

        public RestartState(IGameStateMachine gameStateMachine, ISaveLoadService saveLoadService,
                SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
        }

        public void Enter()
        {
            _loadingCurtain.Show();
            LoadCleanup();
        }

        public void Exit()
        {
        }

        private void LoadCleanup()
        {
            _sceneLoader.Load(Constants.CleanupScene,
                    SceneManager.GetActiveScene().name == Constants.FightTestScene ? LoadFightScene : LoadMainScene);
        }

        private void LoadFightScene()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(Constants.FightTestScene);

        }

        private void LoadMainScene()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(Constants.BattleScene);
        }

        public class Factory : PlaceholderFactory<IGameStateMachine, RestartState>
        {
        }
    }
}