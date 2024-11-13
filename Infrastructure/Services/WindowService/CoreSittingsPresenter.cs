using System;
using Infrastructure.Services.UIModelFactory;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService.MVVM;
using Zenject;

namespace Infrastructure.Services.WindowService
{
    public sealed class CoreSittingsPresenter : IDisposable 
    {
        private  CoreSittingsView _view;
        private readonly IUIViewFactory _uiviewFactory;
        private readonly IInstantiator _instantiator;
        private readonly IUIModelFactory _modelFactory;

        public CoreSittingsPresenter(IUIViewFactory uiviewFactory, IInstantiator instantiator)
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
            CoreSittingsModel menuViewModel = _instantiator.Instantiate<CoreSittingsModel>();
            _view.Initialize(menuViewModel);
            _view.SetActive(true);
        }

        public void Dispose()
        {
            if (_view == null) return;
            _view.Dispose();
        }

        private CoreSittingsView CreateView()
        {
            CoreSittingsView menuView = _uiviewFactory.CreateCoreSittingsView();
            return menuView;
        }
    }
}