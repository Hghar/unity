using System;
using Zenject;

namespace Battle
{
    public class BattleContinuingFlag : IDisposable, IBattleContinuingFlag
    {
        private IBattleStartPublisher _battleStartPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;
        private bool _value;

        public bool Value => _value;

        [Inject]
        private void Construct(IBattleStartPublisher battleStartPublisher, IBattleFinishPublisher battleFinishPublisher)
        {
            _battleStartPublisher = battleStartPublisher;
            _battleFinishPublisher = battleFinishPublisher;

            _battleStartPublisher.BattleStarted += OnBattleStarted;
            _battleFinishPublisher.BattleFinished += OnBattleFinished;
        }

        private void OnBattleStarted()
        {
            _value = true;
        }

        private void OnBattleFinished()
        {
            _value = false;
            ;
        }

        public void Dispose()
        {
            if (_battleStartPublisher != null)
                _battleStartPublisher.BattleStarted -= OnBattleStarted;

            if (_battleFinishPublisher != null)
                _battleFinishPublisher.BattleFinished -= OnBattleFinished;
        }
    }
}