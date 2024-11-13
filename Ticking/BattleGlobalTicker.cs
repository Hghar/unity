using Battle;
using System.Collections;
using NaughtyAttributes;
using Realization.Configs;
using Realization.States.CharacterSheet;
using UnityEngine;
using Zenject;
using Constants = Realization.Configs.Constants;

namespace Ticking
{
    public class BattleGlobalTicker : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField] private float _cooldown = 1f;

        private IGlobalTickable _tickablePool;
        private IBattleStartPublisher _battleStartPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;
        private Coroutine _ticking;
        private Constants _constants;

        [Inject]
        private void Construct(IGlobalTickable tickablePool, 
            IBattleStartPublisher battleStartPublisher, 
            IBattleFinishPublisher battleFinishPublisher,
            Constants characterConfig)
        {
            _constants = characterConfig;
            _tickablePool = tickablePool;
            _battleStartPublisher = battleStartPublisher;
            _battleFinishPublisher = battleFinishPublisher;
        }

        private void OnEnable()
        {
            _battleStartPublisher.BattleStarted += OnBattleStarted;
            _battleFinishPublisher.BattleFinished += OnBattleFinished;
            _cooldown = _constants.GeneralClockFrequency;
        }

        private void OnDisable()
        {
            _battleStartPublisher.BattleStarted -= OnBattleStarted;
            _battleFinishPublisher.BattleFinished -= OnBattleFinished;
        }

        private void OnBattleStarted()
        {
            _ticking = StartCoroutine(Ticking());
        }

        private void OnBattleFinished()
        {
            if (_ticking != null)
                StopCoroutine(_ticking);
        }

        private IEnumerator Ticking()
        {
            while (true)
            {
                _tickablePool.Tick();
                yield return new WaitForSeconds(_cooldown);
            }
        }
    }
}