using Helpers;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class DuringBattleDisabler : MonoBehaviour
    {
        [SerializeField] private BehaviourSwitcher _switcher;

        private IBattleStartPublisher _battleStartPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;
        private IBattleContinuingFlag _battleContinuingFlag;

        [Inject]
        private void Construct(
            IBattleStartPublisher battleStartPublisher,
            IBattleFinishPublisher battleFinishPublisher,
            IBattleContinuingFlag battleContinuingFlag)
        {
            _battleStartPublisher = battleStartPublisher;
            _battleFinishPublisher = battleFinishPublisher;
            _battleContinuingFlag = battleContinuingFlag;
        }

        public void TestConstruct(BehaviourSwitcher draggable)
        {
            _switcher = draggable;
        }

        private void OnEnable()
        {
            if (_switcher != null)
            {
                if (_battleContinuingFlag.Value)
                    _switcher.SwitchOff();
                else
                    _switcher.SwitchOn();
            }

            if(_battleStartPublisher != null && _battleFinishPublisher != null)
            {
                _battleStartPublisher.BattleStarted += OnBattleStarted;
                _battleFinishPublisher.BattleFinished += OnBattleFinished;
            }
        }

        private void OnDisable()
        {
            if (_switcher != null) 
                _switcher.SwitchOn();
            
            if(_battleStartPublisher != null && _battleFinishPublisher != null)
            {
                _battleStartPublisher.BattleStarted += OnBattleStarted;
                _battleFinishPublisher.BattleFinished += OnBattleFinished;
            }
        }

        private void OnBattleFinished()
        {
            _switcher.SwitchOn();
        }

        private void OnBattleStarted()
        {
            _switcher.SwitchOff();
        }
    }
}