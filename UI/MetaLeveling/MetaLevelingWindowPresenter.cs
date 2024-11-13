using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Infrastructure.Services.UIModelFactory;
using Infrastructure.Services.UIViewFactory;
using Infrastructure.Services.WindowService.MVVM;

namespace Infrastructure.Services.WindowService
{
    public sealed class MetaLevelingWindowPresenter : IDisposable
    {
        private LevelingView _view;
        private readonly IUIViewFactory _uiviewFactory;
        private readonly IUIModelFactory _modelFactory;

        public MetaLevelingWindowPresenter(IUIViewFactory uiviewFactory, IUIModelFactory modelFactory)
        {
            _uiviewFactory = uiviewFactory;
            _modelFactory = modelFactory;
        }

        public void ShowWindow()
        {
            LevelingViewModel levelingViewModel = _modelFactory.CreateLevelingViewModel();
            _view.Initialize(levelingViewModel);
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

        private LevelingView CreateView()
        {
            LevelingView menuView = _uiviewFactory.CreateLevelingWindowView();
            return menuView;
        }

        public void Init()
        {
            _view = CreateView();
            _view.SetActive(false);
        }
    }
}