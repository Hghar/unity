using System;
using Battle;
using Infrastructure.CompositeDirector.Composites;
using Model.Composites.Representation;
using Model.Maps;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Realization.Map
{
    public class MapMover : MonoBehaviour
    {
        private IBattleFinishPublisher _battleFinishPublisher;
        private IMap _map;
        private Composite<IRepresentation> _representation;

        [Inject]
        private void Construct(IBattleFinishPublisher battleFinishPublisher, IMap map,
            Composite<IRepresentation> representation)
        {
            _representation = representation;
            _map = map;
            _battleFinishPublisher = battleFinishPublisher;
        }

        private void Awake()
        {
            _battleFinishPublisher.BattleFinishedAndReadyToMove += MoveToNextRoom;
        }

        private void MoveToNextRoom()
        {
            _map.MoveTo(new Vector2(1, 0));
            _representation.Select().ForAll().Do().Represent();
        }

        [Button]
        public void Next()
        {
            _map.MoveTo(new Vector2(1, 0));
        }
    }
}