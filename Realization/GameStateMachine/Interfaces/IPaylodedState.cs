namespace Realization.GameStateMachine.Interfaces
{
    public interface IPaylodedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}