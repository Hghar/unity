using System;
using Infrastructure.Services.UIModelFactory;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService.MVVM;
using Zenject;

namespace Infrastructure.Services.WindowService
{
    public sealed class LoseScreenViewPresenter : IDisposable 
    {
        private  LoseScreenView _view;
        private readonly IUIViewFactory _uiviewFactory;
        private readonly IInstantiator _instantiator;
        private readonly IUIModelFactory _modelFactory;

        public LoseScreenViewPresenter(IUIViewFactory uiviewFactory, IInstantiator instantiator)
        {
            _uiviewFactory = uiviewFactory;
            _instantiator = instantiator;
        }

        public void Init()
        {
            _view = CreateView();
            _view.SetActive(false);
        }

        public void HideWindow()
        {
            _view.ClearViewModel();
            _view.SetActive(false);
        }

        public void ShowWindow()
        {
            Init();
            LoseScreenViewModel menuViewViewModel = _instantiator.Instantiate<LoseScreenViewModel>();
            _view.Initialize(menuViewViewModel);
            _view.SetActive(true);
            _view.OpenAnimation();
        }

        public void Dispose()
        {
            if (_view == null) return;
            _view.Dispose();
        }

        private LoseScreenView CreateView()
        {
            LoseScreenView menuView = _uiviewFactory.CreateLoseScreen();
            return menuView;
        }
    }
}