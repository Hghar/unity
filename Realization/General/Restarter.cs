using System;
using DefaultNamespace;
using Realization.GameStateMachine.Interfaces;
using Realization.GameStateMachine.States;
using UI.Restarting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.General
{
    public class Restarter : IRestarter, IDisposable
    {
        private IRestartButton _restartButton;
        private IGameStateMachine _statemachine;

        public event Action Restarting;

        [Inject]
        private void Construct(IRestartButton restartButton,IGameStateMachine stateMachine)
        {
            _statemachine = stateMachine;
            Debug.Log("Restarter Construct!");
            _restartButton = restartButton;
            _restartButton.Clicked += OnRestartButtonClicked;
        }

        public void Dispose()
        {
            _restartButton.Clicked -= OnRestartButtonClicked;
        }

        private void OnRestartButtonClicked()
        {
            Restarting?.Invoke();
            _statemachine.Enter<RestartState>();
            
            /*SceneManager.LoadScene(SceneManager.GetActiveScene()
                .name); // TODO: refactor state machine and remove duplicates*/
        }
    }
}