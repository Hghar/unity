using System;
using Zenject;

namespace Battle
{
    public class BattleStarter : IBattleStartPublisher, IDisposable
    {
        private IStartBattleButton _startBattleButton;

        public event Action BattleStarted;
        public event Action BeforeBattleStarted;

        [Inject]
        private void Construct(IStartBattleButton startBattleButton)
        {
            _startBattleButton = startBattleButton;
            _startBattleButton.Clicked += OnButtonClicked;
        }

        public void Dispose()
        {
            BattleStarted = null;
            BeforeBattleStarted = null;
            //_startBattleButton.Clicked -= OnButtonClicked; TODO: unsubscribe
        }

        private void OnButtonClicked()
        {
            BeforeBattleStarted?.Invoke();
            BattleStarted?.Invoke();
        }
    }
}