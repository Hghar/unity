using System;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using UnityEngine;
using Zenject;

namespace Battle
{
    public class BattleFinisher : IBattleFinishPublisher, IDisposable
    {
        public event Action BattleFinished;
        public event Action BattleFinished1;
        public event Action BattleFinishedAndReadyToMove;

        private IAllEnemiesDeadPublisher _allEnemyDeadPublisher;
        private Composite<IRepresentation> _representation;

        [Inject]
        private void Construct(IAllEnemiesDeadPublisher allEnemyDeadPublisher,
            Composite<IRepresentation> representation)
        {
            _representation = representation;
            _allEnemyDeadPublisher = allEnemyDeadPublisher;
            _allEnemyDeadPublisher.AllEnemiesDead += OnAllEnemiesDead;
        }

        public void Dispose()
        {
            _allEnemyDeadPublisher.AllEnemiesDead -= OnAllEnemiesDead;
            BattleFinished = null;
            BattleFinished1 = null;
            BattleFinishedAndReadyToMove = null;
        }

        private void OnAllEnemiesDead()
        {
            _representation.Select().ForAll().Do().Represent();
            
            BattleFinished?.Invoke();
            BattleFinished1?.Invoke();
            BattleFinishedAndReadyToMove?.Invoke();
        }
    }
}