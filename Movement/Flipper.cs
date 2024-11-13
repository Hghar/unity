using System;
using Battle;
using Fight.Targeting;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Movement
{
    public class Flipper : MonoBehaviour
    {
        [SerializeField] private Transform _parentObject;
        [SerializeField] private FightTargetFinder _targetFinder;
        private IBattleFinishPublisher _battleFinishPublisher;
        private Vector3 _defaultSize = new Vector3(1,1,1);
        public bool Flipped => _parentObject.localScale != _defaultSize;

        [Inject]
        private void Construct(IBattleFinishPublisher battleFinishPublisher)
        {
            _battleFinishPublisher = battleFinishPublisher;
            _battleFinishPublisher.BattleFinished += OnBattleFinished;
        }

        public void SetDefaultSize(Vector3 vector3)
        {
            _defaultSize = vector3;
        }

        private void Update()
        {
            if (TargetIsAlive() && TargetIsNotDragged()) 
                Rotate();
        }

        private void OnDestroy()
        {
            _battleFinishPublisher.BattleFinished -= OnBattleFinished;
        }

        private bool TargetIsAlive() =>
                _targetFinder.MinionTarget != null;

        private bool TargetIsNotDragged() =>
                !_targetFinder.MinionTarget.IsDragging;

        private void Rotate()
        {
            if (_targetFinder.MinionTarget == null &&
                this.GetComponent<IMinion>().Fraction == Fight.Fractions.Fraction.Enemies)
            {
                _parentObject.localScale = new Vector3(-_defaultSize.x, _defaultSize.y, _defaultSize.z);
            }
            else if (_targetFinder.MinionTarget == null)
            {
                _parentObject.localScale = new Vector3(_defaultSize.x,  _defaultSize.y, _defaultSize.z);
            }

            if (_targetFinder.MinionTarget == null || _targetFinder.MinionTarget.GameObject == null)
                return;

            if (_targetFinder.MinionTarget.GameObject.transform.position.x > transform.position.x)
                _parentObject.localScale = new Vector3(_defaultSize.x,  _defaultSize.y, _defaultSize.z);
            else if (_targetFinder.MinionTarget.GameObject.transform.position.x < transform.position.x)
                _parentObject.localScale = new Vector3(-_defaultSize.x,  _defaultSize.y, _defaultSize.z);
        }

        private void OnBattleFinished()
        {
            transform.localScale = new Vector3(_defaultSize.x,  _defaultSize.y, _defaultSize.z);
        }
    }
}