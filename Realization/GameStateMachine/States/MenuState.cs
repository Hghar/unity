using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Infrastructure.Helpers;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService;
using Installers;
using Model.Economy;
using Plugins.Ship.Sheets.StepSheet;
using Plugins.Ship.Sheets.StepSheet.Commands;
using Plugins.Ship.Sheets.StepSheet.Commands.DefaultCommands;
using Plugins.Ship.Sheets.StepSheet.Steps;
using Realization.GameStateMachine.Interfaces;
using Realization.General;
using Realization.Shops;
using Realization.States.CharacterSheet;
using Realization.TutorialRealization.Commands;
using Realization.TutorialRealization.Helpers;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class MenuState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISaveLoadService _saveLoadService;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly IUIViewFactory _iuiViewFactory;
        private readonly IWindowService _windowService;
        private INotificationService _notificationService;

        public MenuState(IGameStateMachine gameStateMachine, ISaveLoadService saveLoadService, SceneLoader sceneLoader,
                LoadingCurtain loadingCurtain, IUIViewFactory iuiViewFactory,IWindowService windowService, 
                INotificationService notificationService)
        {
            _notificationService = notificationService;
            _gameStateMachine = gameStateMachine;
            _saveLoadService = saveLoadService;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _iuiViewFactory = iuiViewFactory;
            _windowService = windowService;
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.Menu, Onloaded);
        }

        private void Onloaded()
        {
            _iuiViewFactory.CreateUIRoot();
            _windowService.InitMeta();
            _windowService.Open(WindowId.MainMenuWindow);
            _loadingCurtain.Hide();
            _notificationService.CreateRoot();
            SceneObjectPool.Init();
        }

        public void Exit()
        {
            _loadingCurtain.Show();
            _windowService.ClearMeta();
        }


        public class Factory : PlaceholderFactory<IGameStateMachine, MenuState>
        {
        }
    }

    public class TutorialService : ITickable
    {
        private const string CharacterConfigPath = "Characters Config";
        
        private bool _working;
        private DiContainer _container;
        private TutorialHelpers _tutorialHelpers;
        private CharacterConfig _characterConfig;
        private StepSheet _sheet;
        private INotificationService _notificationService;
        private IStorage _storage;
        private ICoroutineRunner _coroutineRunner;
        private ISaveLoadService _saveLoadService;

        public TutorialService(DiContainer container, INotificationService notificationService, IStorage storage,
            ICoroutineRunner coroutineRunner, ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
            _coroutineRunner = coroutineRunner;
            _storage = storage;
            _notificationService = notificationService;
            _container = container;
        }
        
        public void Init()
        {
            _tutorialHelpers = _container.InstantiatePrefabResourceForComponent<TutorialHelpers>("Tutorial/TutorialHelpers");
            _characterConfig = Resources
                .Load<CharacterConfig>(CharacterConfigPath);
            
            var unityConditions = new UnityConditions(_storage);
            IConditionDictionary[] conditions =
            {
                new DefaultConditions(),
                unityConditions,
            };
            IActionDictionary[] actions =
            {
                new DefaultActions(),
                new UnityActions(_tutorialHelpers.Finder, _tutorialHelpers.Tutorial, _container, _notificationService,
                    _storage, _saveLoadService),
            };
            var nameConverter = new NameConverter(conditions, actions);
            var builder = new NodeBuilder(nameConverter);
            var tutorialNodes = _characterConfig.TutorialNodes;
            var steps = new List<IStep>();
            foreach (var tutorialNode in tutorialNodes)       
            {
                string[] ids = tutorialNode.Uid.Split("&");
                builder.Perform(ids);
                string[] startConditions = tutorialNode.StartConditions.Split("&");
                builder.Perform(startConditions);
                string[] stepActions = tutorialNode.Actions.Split("&");
                builder.Perform(stepActions);
                string[] endConditions = tutorialNode.EndConditions.Split("&");
                builder.Perform(endConditions);
                steps.Add(builder.Get());
            }

            _sheet = new StepSheet("tutorial", steps);
        }

        public async void Start()
        {
            _working = true;
            _sheet.Start();
            
            while (_working && _sheet != null && _sheet.Working)
            {
                await _sheet.Update(_coroutineRunner as MonoBehaviour);
                await UniTask.WaitForEndOfFrame(_coroutineRunner as MonoBehaviour);
            }
        }

        public void Tick()
        {
            // if(!_working || _sheet == null || !_sheet.Working)
            //     return;
            //
            // await _sheet.Update();
        }

        public void Clear()
        {
            // Object.Destroy(_tutorialHelpers.gameObject);
        }
    }
}