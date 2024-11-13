using Infrastructure.Services.CharacteristicSetupService;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.RandomService;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.SceneLoader;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StatsBoostService;
using Infrastructure.Services.TickInvokerService;
using Infrastructure.Services.UIModelFactory;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService;
using Infrastructure.Services.WindowService.MVVM;
using Infrastructure.Services.WindowService.PrefabFactory;
using Model.Economy;
using Realization.Economy;
using Realization.GameStateMachine;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using Zenject;
using ViewFactory = Infrastructure.Services.WindowService.ViewFactory.ViewFactory;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindPrefabFactory();
            BindViewFactory();
            BindNotificationService();
            BindTickInvoker();
            BindGameBootstrapperFactory();
            BindCoroutineRunner();
            BindLoadingCurtain();
            BindStorage();
            BindSaveLoadService();
            BindRandomService();
            BindStaticDataService();
            BindCharacteristicSetupService();
            BindStatsBoostService();
            BindSceneLoader();
            BindUIFactory();
            BindUIModels();
            BindUIModelFactory();
            BindWindowService();
            BindHudFactory();
            BindGameStateMachine();
            Container.BindInterfacesAndSelfTo<TutorialService>().AsSingle();
           

            Container.Bind(typeof(IGameStateMachine), typeof(ITickable), typeof(IFixedTickable))
                    .To<GameStateMachine>().AsSingle();
        }

        private void BindTickInvoker() => Container.BindInterfacesAndSelfTo<TickInvoker>().AsSingle();

        private void BindViewFactory() => Container.BindInterfacesTo<ViewFactory>().AsSingle();

        private void BindPrefabFactory() => Container.BindInterfacesTo<PrefabFactory>().AsSingle();

        private void BindUIModels()
        {
            Container.Bind<LevelingViewModel>().AsTransient();
            Container.Bind<MenuViewModel>().AsTransient();
            Container.Bind<WinScreenViewModel>().AsTransient();
            Container.Bind<LoseScreenViewModel>().AsTransient();
            Container.Bind<BiomItemsViewModel>().AsTransient();
            Container.Bind<MenuItemViewModel>().AsTransient();
            Container.Bind<CoreSittingsModel>().AsTransient();
            Container.Bind<MenuWindowPresenter>().AsSingle();
            Container.Bind<CoreSittingsPresenter>().AsSingle();
            Container.Bind<WinScreenViewPresenter>().AsSingle();
            Container.Bind<LoseScreenViewPresenter>().AsSingle();
            Container.Bind<MetaLevelingWindowPresenter>().AsSingle();
        }
        private void BindGameStateMachine()
        {
            Container.BindFactory<IGameStateMachine, BootstrapState, BootstrapState.Factory>();
            Container.BindFactory<IGameStateMachine, LoadProgressState, LoadProgressState.Factory>();
            Container.BindFactory<IGameStateMachine, WinState, WinState.Factory>();
            Container.BindFactory<IGameStateMachine, LoseState, LoseState.Factory>();
            Container.BindFactory<IGameStateMachine, GameloopState, GameloopState.Factory>();
            Container.BindFactory<IGameStateMachine, LoadLevelState, LoadLevelState.Factory>();
            Container.BindFactory<IGameStateMachine, RestartState, RestartState.Factory>();
            Container.BindFactory<IGameStateMachine, MenuState, MenuState.Factory>();
        }

        private void BindLoadingCurtain() => 
                Container.Bind<LoadingCurtain>().FromComponentInNewPrefabResource(Path.CurtainPath).AsSingle();
        private void BindGameBootstrapperFactory() =>
                Container
                        .BindFactory<GameBootstrapper, GameBootstrapper.Factory>()
                        .FromComponentInNewPrefabResource(Path.GameBootstraper);

        private void BindStorage() => Container.Bind<IStorage>().To<Storage>().AsSingle();
        private void BindWindowService() => Container.Bind<IWindowService>().To<WindowService>().AsSingle();

        private void BindStaticDataService() => Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
        private void BindStatsBoostService() => Container.Bind<IStatsBoostService>().To<StatsBoostService>().AsSingle();
        private void BindCharacteristicSetupService() => Container.Bind<ICharacteristicSetupService>().To<CharacteristicSetupService>().AsSingle();
        private void BindRandomService() => Container.Bind<IRandomService>().To<RandomService>().AsSingle();
        private void BindNotificationService() => Container.Bind<INotificationService>().To<NotificationService>().AsSingle();

        private void BindSaveLoadService() => Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
        private void BindUIFactory() => Container.Bind<IUIViewFactory>().To<UIViewFactory>().AsSingle();
        private void BindUIModelFactory() => Container.Bind<IUIModelFactory>().To<UIModelFactory>().AsSingle();
        private void BindHudFactory() => Container.Bind<IHudFactory>().To<HudFactory>().AsSingle();
        
        private void BindCoroutineRunner() =>
                Container
                        .Bind<ICoroutineRunner>()
                        .To<CoroutineRunner>()
                        .FromComponentInNewPrefabResource(Path.CoroutineRunner)
                        .AsSingle();
        
        private void BindSceneLoader() => Container.Bind<SceneLoader>().AsSingle();
        
    }
}