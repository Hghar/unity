using Infrastructure.Services.StaticData;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;

namespace Infrastructure.Services.WindowService.MVVM
{
    public sealed class LoseScreenViewModel : EmptyViewModel
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly LoseScreenViewPresenter _winScreenViewPresenter;
        
        public LoseScreenViewModel(IGameStateMachine gameStateMachine,LoseScreenViewPresenter winScreenViewPresenter)
        {
            _gameStateMachine = gameStateMachine;
            _winScreenViewPresenter = winScreenViewPresenter;
        }
        public void OnCloseClick()
        {
            _winScreenViewPresenter.HideWindow();
            _gameStateMachine.Enter<MenuState>();

        }
    }

    public sealed class WinScreenViewModel : EmptyViewModel
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly WinScreenViewPresenter _winScreenViewPresenter;
        public int GoldCount => _gold;
        public int CrystalCount => _crystal;
        public int TokenCount => _token;

        private int _gold;
        private int _crystal;
        private int _token;
        
        public WinScreenViewModel(IGameStateMachine gameStateMachine,IStaticDataService staticDataService,WinScreenViewPresenter winScreenViewPresenter)
        {
            _gameStateMachine = gameStateMachine;
            _staticDataService = staticDataService;
            _winScreenViewPresenter = winScreenViewPresenter;
        }
        public void OnCloseClick()
        {
            _winScreenViewPresenter.HideWindow();
            _gameStateMachine.Enter<MenuState>();

        }
    }
}