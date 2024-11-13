using System;
using Infrastructure.Services.UIModelFactory;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService.MVVM;

namespace Infrastructure.Services.WindowService
{
    public sealed class MenuWindowPresenter : IDisposable 
    {
        private  MenuView _view;
        private readonly IUIViewFactory _uiviewFactory;
        private readonly IUIModelFactory _modelFactory;

        public MenuWindowPresenter(IUIViewFactory uiviewFactory, IUIModelFactory modelFactory)
        {
            _uiviewFactory = uiviewFactory;
            _modelFactory = modelFactory;
        }
        public void Init()
        {
            _view = CreateView();
            _view.SetActive(false);
        }
        public void ShowWindow()
        {
            MenuViewModel menuViewModel = _modelFactory.CreateMenuViewModel();
            _view.Initialize(menuViewModel);
            _view.SetActive(true);
        }

        public void HideWindow()
        {
            _view.ClearViewModel();
            _view.SetActive(false);
        }

        public void Dispose()
        {
            _view.Dispose();
        }

        private MenuView CreateView()
        {
            MenuView menuView = _uiviewFactory.CreateMenuWindowView();
            return menuView;
        }

        
    }
}