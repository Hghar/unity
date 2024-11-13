using System;
using System.Collections;
using Fight.Targeting;
using UnityEngine;

namespace Movement.Pathfinders
{
    public class ToEnemyPathfinder : MonoBehaviour, IPathfinder
    {
        [SerializeField] private FightTargetFinder _fightTargetFinder;
        [SerializeField] private float _minDistanceToFight = 0.05f;

        private Coroutine _computingWayToFight;
        private Vector2 _directionToFight;
        private float _minSqrDistanceToFight;

        public event Action TargetFound;
        public event Action TargetLost;

        private void Awake()
        {
            _minSqrDistanceToFight = _minDistanceToFight * _minDistanceToFight;
        }

        private void OnEnable()
        {
            _fightTargetFinder.TargetFound += OnTargetFound;
        }

        private void OnDisable()
        {
            _fightTargetFinder.TargetFound -= OnTargetFound;
        }

        public Vector2 ComputeDirection()
        {
            if (_computingWayToFight == null)
                return Vector2.zero;

            return _directionToFight;
        }

        public bool HasTarget()
        {
            return _computingWayToFight != null;
        }

        private void OnTargetFound()
        {
            if (_computingWayToFight == null)
            {
                _computingWayToFight = StartCoroutine(ComputingWayToFight());
                TargetFound?.Invoke();
            }
        }

        private IEnumerator ComputingWayToFight()
        {
            while (_fightTargetFinder.ChosenTarget != null)
            {
                Vector2 toFight = _fightTargetFinder.ChosenTarget.WorldPosition - (Vector2) transform.position;
                if (_minSqrDistanceToFight < toFight.sqrMagnitude)
                    _directionToFight = toFight.normalized;
                else
                    _directionToFight = Vector2.zero;

                yield return new WaitForFixedUpdate();
            }

            _computingWayToFight = null;
            TargetLost?.Invoke();
        }
    }
}