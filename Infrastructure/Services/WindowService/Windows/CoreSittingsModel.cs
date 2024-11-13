using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;

namespace Infrastructure.Services.WindowService.MVVM
{
    public sealed class CoreSittingsModel : EmptyViewModel
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly CoreSittingsPresenter _coreSittingsPresenter;

        public CoreSittingsModel(IGameStateMachine gameStateMachine,CoreSittingsPresenter coreSittingsPresenter)
        {
            _gameStateMachine = gameStateMachine;
            _coreSittingsPresenter = coreSittingsPresenter;
        }
        public void OnMenuClick()
        {
            _gameStateMachine.Enter<MenuState>();
        }

        public void OnRestartClick()
        {
            _gameStateMachine.Enter<RestartState>();
        }

        public void OnCloseClick()
        {
            _coreSittingsPresenter.HideWindow();
            _coreSittingsPresenter.Dispose();
        }
    }
}