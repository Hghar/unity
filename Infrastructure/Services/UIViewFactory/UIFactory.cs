using System.Collections.Generic;
using DefaultNamespace;
using Infrastructure.Helpers;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.WindowService;
using Infrastructure.Services.WindowService.MVVM;
using Infrastructure.Services.WindowService.ViewFactory;
using Model.Economy;
using Realization.UI;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.UIViewFactory
{
    public class UIViewFactory:IUIViewFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;
        private readonly IStorage _storage;
        private readonly IViewFactory _viewFactory;
        private Transform _uiRoot;
        private const string UIRootPath = "WindowPrefabs/UIRoot";

        public UIViewFactory(DiContainer container,IStaticDataService staticDataService,IStorage storage,IViewFactory viewFactory)
        {
            _container = container;
            _staticDataService = staticDataService;
            _storage = storage;
            _viewFactory = viewFactory;
        }
        public void CreateUIRoot()
        {
            var gameObject = Resources.Load<GameObject>(UIRootPath);
            _uiRoot = Object.Instantiate(gameObject).transform;
        }


        public MenuView CreateMenuWindowView()
        {
            WindowData windowData = _staticDataService.ForWindow(WindowId.MainMenuWindow);
            MenuView menuView = _viewFactory.CreateView<MenuView, MenuHierarchy>(windowData.Prefab,_uiRoot);
            return menuView;
        }

        public LoseScreenView CreateLoseScreen()
        {
            WindowData windowData = _staticDataService.ForWindow(WindowId.LosePanel);
            LoseScreenView menuView = _viewFactory.CreateView<LoseScreenView, LoseScreenHierarchy>(windowData.Prefab,_uiRoot);
            SceneObjectPool.Instance.Objects.Add(menuView.Hierarchy.gameObject);
            return menuView;
        }
       
        public LevelingView CreateLevelingWindowView()
        {
            WindowData windowData = _staticDataService.ForWindow(WindowId.MetaLeveling);
            LevelingView levelinView = _viewFactory.CreateView<LevelingView, LevelingMenuHierarchy>(windowData.Prefab, _uiRoot);
            return levelinView;
        }
        private GameObject CreateGamePanel(WindowData windowData)
        {
            var gamepanel = Object.Instantiate(windowData.Prefab );
            SceneObjectPool.Instance.Objects.Add(gamepanel);
            return gamepanel;
        } 
        public CoreSittingsView CreateCoreSittingsView()
        {
            WindowData windowData = _staticDataService.ForWindow(WindowId.CoreSittings);
            CoreSittingsView menuView = _viewFactory.CreateView<CoreSittingsView, CoreSittingsHierarchy>(windowData.Prefab,_uiRoot);
            return menuView;
        }
        public WinScreenView CreateWinScreenView()
        {
            WindowData windowData = _staticDataService.ForWindow(WindowId.WinScreen);
            WinScreenView menuView = _viewFactory.CreateView<WinScreenView, WinScreenHierarchy>(windowData.Prefab,_uiRoot);
            SceneObjectPool.Instance.Objects.Add(menuView.Hierarchy.gameObject);
            return menuView;
        }
    }
}