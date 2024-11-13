using System;
using System.Collections.Generic;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using Zenject;

namespace Realization.GameStateMachine
{
    public class GameStateMachine : IGameStateMachine, ITickable,IFixedTickable
    {
        private Dictionary<System.Type, IExitableState> registeredStates;
        
        private IExitableState currentState;
        private ITickable currentTickableState;
        private IFixedTickable currentFixedTickableState;

        public GameStateMachine(
                BootstrapState.Factory bootstrapStateFactory,
                WinState.Factory winStateFactory,
                GameloopState.Factory playingStateFactory,
                LoadLevelState.Factory startStateFactory,
                LoadProgressState.Factory loadProgressFactory,
                RestartState.Factory restartStateFactory,
                MenuState.Factory menuStateFactory,
                LoseState.Factory createProgressFactory
                )
        {
            registeredStates = new Dictionary<Type, IExitableState>();
            
            RegisterState(bootstrapStateFactory.Create(this));
            RegisterState(startStateFactory.Create(this));
            RegisterState(playingStateFactory.Create(this));
            RegisterState(winStateFactory.Create(this));
            RegisterState(loadProgressFactory.Create(this));
            RegisterState(restartStateFactory.Create(this));
            RegisterState(menuStateFactory.Create(this));
            RegisterState(createProgressFactory.Create(this));
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            TState newState = ChangeState<TState>();
            newState.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, Interfaces.IPaylodedState<TPayload>
        {
            TState newState = ChangeState<TState>();
            newState.Enter(payload);
        }

        private void RegisterState<TState>(TState state) where TState : IExitableState =>
                registeredStates.Add(typeof(TState), state);

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            currentState?.Exit();
      
            TState state = GetState<TState>();
            currentState = state;
            currentTickableState = currentState as ITickable;
            currentFixedTickableState = currentState as IFixedTickable;
      
            return state;
        }
    
        private TState GetState<TState>() where TState : class, IExitableState => 
                registeredStates[typeof(TState)] as TState;

        public void Tick()
        {
            //currentTickableState?.Tick();
        }

        public void FixedTick()
        {
            //currentFixedTickableState?.FixedTick();
        }
    }
}