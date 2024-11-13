using Infrastructure.Services.WindowService;
using Realization.GameStateMachine.Interfaces;
using Zenject;

namespace Realization.GameStateMachine.States
{
    public class GameloopState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IWindowService _windowService;

        public GameloopState(IGameStateMachine gameStateMachine,IWindowService windowService)
        {
            _gameStateMachine = gameStateMachine;
            _windowService = windowService;
        }
        public void Enter()
        {
        }

        public void Exit()
        {
            _windowService.ClearCore();
        }
        public class Factory : PlaceholderFactory<IGameStateMachine, GameloopState>
        {
        }
    }
}