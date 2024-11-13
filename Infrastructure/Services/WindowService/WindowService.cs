namespace Infrastructure.Services.WindowService
{
    public class WindowService : IWindowService
    {
        private readonly MenuWindowPresenter _menuWindowPresenter;

        private readonly MetaLevelingWindowPresenter _metaLevelingWindowPresenter;
        private readonly CoreSittingsPresenter _coreSittingsPresenter;
        private readonly WinScreenViewPresenter _winScreenViewPresenter;
        private readonly LoseScreenViewPresenter _loseScreenViewPresenter;

        public WindowService(MenuWindowPresenter menuWindowPresenter,
                CoreSittingsPresenter coreSittingsPresenter,
                WinScreenViewPresenter winScreenViewPresenter,
                LoseScreenViewPresenter loseScreenViewPresenter,MetaLevelingWindowPresenter metaLevelingWindowPresenter)
        {
            _menuWindowPresenter = menuWindowPresenter;
            _coreSittingsPresenter = coreSittingsPresenter;
            _winScreenViewPresenter = winScreenViewPresenter;
            _loseScreenViewPresenter = loseScreenViewPresenter;
            _metaLevelingWindowPresenter = metaLevelingWindowPresenter;
        }

        public void Open(WindowId windowId)
        {
            switch (windowId)
            {
                case WindowId.MainMenuWindow:
                    _menuWindowPresenter.ShowWindow();
                    break;
                
                case WindowId.MetaLeveling:
                    _metaLevelingWindowPresenter.ShowWindow();
                    break;
                
                case WindowId.CoreSittings:
                    _coreSittingsPresenter.ShowWindow();
                    break;
                
                case WindowId.WinScreen:
                    _winScreenViewPresenter.ShowWindow();
                    break;
                
                case WindowId.LosePanel:
                    _loseScreenViewPresenter.ShowWindow();
                    break;
            }
        }

        public void InitMeta()
        {
            _menuWindowPresenter.Init();
            _metaLevelingWindowPresenter.Init();
        }

        public void InitCore()
        {
            _coreSittingsPresenter.Init();
        }

        public void ClearMeta()
        {
            _menuWindowPresenter.HideWindow();
            _menuWindowPresenter.Dispose();
            _metaLevelingWindowPresenter.HideWindow();
            _metaLevelingWindowPresenter.Dispose();

            
        }
        public void ClearCore()
        {
            _coreSittingsPresenter.HideWindow();
            _coreSittingsPresenter.Dispose();
        }
    }
}