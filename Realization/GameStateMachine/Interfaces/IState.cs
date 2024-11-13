namespace Realization.GameStateMachine.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}