using Fight.Attack;
using Movement;
using Pathfinding;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class BattleStartHandler : MonoBehaviour
    {
        [SerializeField] private Attacker _attacker;
        [SerializeField] private AILerp _aILerp;

        IBattleStartPublisher _battleStartPublisher;
        IBattleFinishPublisher _battleFinishPublisher;
        private bool _started;

        [Inject]
        private void Construct(IBattleStartPublisher battleStarter, IBattleFinishPublisher battleFinishPublisher)
        {
            _battleStartPublisher = battleStarter;
            _battleStartPublisher.BattleStarted += OnBattleStarted;

            _battleFinishPublisher = battleFinishPublisher;
            _battleFinishPublisher.BattleFinished += OnBattleFinished;
        }

        void Start()
        {
            if(_started == false)
                DisableFightBehaviours();
        }

        private void OnDestroy()
        {
            if (_battleStartPublisher == null)
                return;
            _battleStartPublisher.BattleStarted -= OnBattleStarted;
            _battleFinishPublisher.BattleFinished -= OnBattleFinished;
        }

        private void OnBattleStarted()
        {
            EnableFightBehaviours();
        }

        private void OnBattleFinished()
        {
            DisableFightBehaviours();
        }

        private void EnableFightBehaviours()
        {
            

            _attacker.enabled = true;

            if (_aILerp != null)
                _aILerp.enabled = false;
        }

        private void DisableFightBehaviours()
        {
            _attacker.enabled = false;

            // if (_aILerp != null)
            //     _aILerp.enabled = true;
        }

        public void StartBattle()
        {
             _started = true;
            OnBattleStarted();
        }
    }
}