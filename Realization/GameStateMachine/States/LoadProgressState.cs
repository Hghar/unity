using System;
using DefaultNamespace;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Model.Economy.Resources;
using Realization.GameStateMachine.Interfaces;
using Realization.States;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStorage _storage;
        private readonly IStaticDataService _staticDataService;
        private readonly StateWorker _stateWorker;

        public LoadProgressState(IGameStateMachine gameStateMachine, 
            ISaveLoadService saveLoadService, 
            IStorage storage, 
            IStaticDataService staticDataService)
        {
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _storage = storage;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _storage.PlayerProgress = _saveLoadService.LoadProgress() ?? NewProgress();

            var stateWorker = UnityEngine.Object.FindObjectOfType<StateWorker>();
            if (stateWorker!=null)stateWorker.Init();
            if (SceneManager.GetActiveScene().name == Constants.FightTestScene)
            {
                _gameStateMachine.Enter<LoadLevelState, string>(Constants.FightTestScene);
                return;
            }

            if (_storage.PlayerProgress.TutorialData.Key == 0)
                _gameStateMachine.Enter<LoadLevelState, string>(Constants.BattleScene);
            else
                _gameStateMachine.Enter<MenuState>();
        }

        private PlayerProgress NewProgress()
        {
            ProgressDefaultSittings defaultSittings = new ProgressDefaultSittings(_staticDataService);
            var defaultProgress = defaultSittings.SetupDefault();
            return defaultProgress;
        }

        public class ProgressDefaultSittings
        {
            private readonly IStaticDataService _staticDataService;

            public ProgressDefaultSittings(IStaticDataService staticDataService)
            {
                _staticDataService = staticDataService;
            }
            public PlayerProgress SetupDefault()
            {
                Configs.Constants constants = _staticDataService.CharacterConfig().Constants;
                ResourceFactory resourceFactory = new ResourceFactory();
                Resource gold = resourceFactory.Create(Currency.Gold);
                gold.Add(constants.TavernBasicAmountOfGold);
                Resource crystals = resourceFactory.Create(Currency.Crystals);
                Resource hard = resourceFactory.Create(Currency.Hard);
                Resource metaGold = resourceFactory.Create(Currency.MetaGold);
                Resource[] resources = { gold, crystals, hard,metaGold};

                PlayerProgress playerProgress = new PlayerProgress();
                playerProgress.Analytics.RegisterTime = DateTime.Now; 
            
            
                Biom biom = new Biom();
                biom.LastPassedStageNumber = 1;
                biom.CurrentRoom = 0;
                biom.Key = 1;
                biom.Shop = new ShopData();
                biom.Shop.Level = 0;
                playerProgress.Bioms.SelectedBiom = biom;
                playerProgress.Bioms.Opened.Add(biom);
                playerProgress.WorldData.CurrencyData.IResources = resources;

                return playerProgress;
            }
        }

        public void Exit()
        {
        }

        public class Factory : PlaceholderFactory<IGameStateMachine, LoadProgressState>
        {
        }
    }
}