using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fight.Fractions;
using Model.Maps;
using Units;
using Units.Ai;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class StartBattleButtonShower : MonoBehaviour
    {
        private IMinionsSetPositionsPublisher _minionsSetPositionsPublisher;
        private IEnemiesSpawnedPublisher _enemiesSpawnedPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;
        private IStartBattleButton _startBattleButton;

        private bool _isEnemiesSpawned;
        private IMap _map;
        private MinionFactory _minionFactory;

        [Inject]
        private void Construct(IMinionsSetPositionsPublisher minionsSetPositionsPublisher,
            IEnemiesSpawnedPublisher enemiesSpawnedPublisher,
            IBattleFinishPublisher battleFinishPublisher,
            IStartBattleButton startBattleButton,
            IMap map,
            MinionFactory minionFactory)
        {
            _minionFactory = minionFactory;
            _map = map;
            _minionsSetPositionsPublisher = minionsSetPositionsPublisher;
            _enemiesSpawnedPublisher = enemiesSpawnedPublisher;
            _battleFinishPublisher = battleFinishPublisher;
            _startBattleButton = startBattleButton;
        }

     

        private void OnEnable()
        {
            // _minionsSetPositionsPublisher.AllMinionsSetPositions += OnMinionsSetPositions;
            _enemiesSpawnedPublisher.EnemiesSpawned += OnEnemiesSpawned;
            _battleFinishPublisher.BattleFinished += OnBattleFinished;
            _startBattleButton.Clicked += OnButtonClicked;
            // _map.Moved += OnMinionsSetPositions;
        }

        private void OnDisable()
        {
            // _minionsSetPositionsPublisher.AllMinionsSetPositions -= OnMinionsSetPositions; // TODO: unsubscribe
            // _enemiesSpawnedPublisher.EnemiesSpawned -= OnEnemiesSpawned;
            // _battleFinishPublisher.BattleFinished -= OnBattleFinished;
            // _startBattleButton.Clicked -= OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            _startBattleButton.SetActive(false);
        }

        private void OnBattleFinished()
        {
            _isEnemiesSpawned = false;
        }

        private void OnEnemiesSpawned()
        {
            // _isEnemiesSpawned = true;
            StartCoroutine(WaitMinionsMoving());
        }

        public IEnumerator WaitMinionsMoving()
        {
            while (NoMinions())
            {
                yield return new WaitForEndOfFrame();
            }
            _startBattleButton.SetActive(true);
        }

        private bool NoMinions()
        {
            IEnumerable<IMinion> enumerable = _minionFactory.Minions.Where(x=>x.Fraction==Fraction.Minions);
            if (enumerable.Count() == 0) return true;
            return !enumerable.Any((minion => minion.IsMoving));
        }
    }
}