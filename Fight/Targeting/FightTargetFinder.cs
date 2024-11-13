using System;
using System.Collections.Generic;
using Battle;
using Extensions;
using Point;
using Units;
using Helpers.Position;
using Parameters;
using UnityEngine;
using Zenject;
using System.Collections;
using System.Linq;

namespace Fight.Targeting
{
    public class FightTargetFinder : MonoBehaviour
    {
        [SerializeField] private Transform _center;
        [SerializeField] private TargetContainer _targets;
        [SerializeField] private bool _isHealing = false; // TODO: separate

        private readonly List<IMinion> _currentPriorityTargets = new List<IMinion>();

        private IMinion _minionTarget;
        private IMinion _minion;
        private IBattleStartPublisher _startPublisher;
        private IBattleFinishPublisher _finishPublisher;
        private IEnumerator _findTargetTimer;

        public IMinion MinionTarget => _minionTarget;
        public IMinion Minion => _minion;

        public event Action TargetFound;

        public IDestroyablePoint ChosenTarget
        {
            get
            {

                if(_minionTarget == null)
                    ChooseTarget(CurrentPriorityTargets);

                return _minionTarget;
            }
        }

        public List<IMinion> CurrentPriorityTargets => _currentPriorityTargets;

        [Inject]
        private void Construct(IBattleStartPublisher startPublisher, IBattleFinishPublisher finishPublisher)
        {
            _startPublisher = startPublisher;
            _finishPublisher = finishPublisher;
            _startPublisher.BattleStarted += FindTargets;
        }

        public void FindTargets()
        {
            if(_targets == null)
                return;
            
            _targets.FindTargets();
            ChooseTarget(CurrentPriorityTargets);
        }
        
        public void FindTargets(List<IMinion> targets)
        {
            if(_targets == null)
                return;
            
            var target = ChooseTarget(targets);
        }

        private void OnDestroy()
        {
            _startPublisher.BattleStarted -= FindTargets;
        }

        private void OnEnable()
        {
            _targets.TargetAdded += OnTargetAdded;
            _targets.TargetRemoved += OnTargetRemoved;

            FindTargets();
        }

        private void Update()
        {
            if (_targets.Targets.ToList().Count == 0)
                FindTargets();
        }

        private void OnDisable()
        {
            _targets.TargetAdded -= OnTargetAdded;
            _targets.TargetRemoved -= OnTargetRemoved;
        }

        public bool TryComputeSqrDistanceToTarget(out float distanceSqr)
        {
            distanceSqr = float.MaxValue;
            if (_minionTarget == null)
                return false;

            distanceSqr = ComputeSqrDistanceToTarget();
            return true;
        }

        public bool TryGetTargetPosition(out Vector2 position)
        {
            position = Vector2.zero;

            if (_minionTarget == null || _minionTarget.Equals(null))
                return false;

            position = _minionTarget.Position;
            return true;
        }

        private void OnTargetAdded(IMinion target)
        {
            TryAddToPriors(target);
            ChooseTarget(CurrentPriorityTargets);
        }

        private void OnTargetRemoved(IMinion target)
        {
            if (_minionTarget == target)
                _minionTarget = null;

            TryRemoveFromPriors(target);
            ChooseTarget(CurrentPriorityTargets);
        }

        private float ComputeSqrDistanceToTarget()
        {
            return _center.ComputeSqrDistanceTo(_minionTarget);
        }

        private bool isTeack = false;
        private bool isEndTeack = true;

        private IMinion ChooseTarget(List<IMinion> targets)
        {
            targets.RemoveAll((minion => minion == null));

            if (targets.Count == 0)
            {
                _minionTarget = null;
                return _minionTarget;
            }

            IMinion previousTarget = _minionTarget;

            float maxPriority = 0;

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].Equals(null))
                    continue;

                if (targets[i].Fraction == _minion.Fraction)
                    continue;

                float targetPriority = CalculateTargetPriority(targets[i]);

                if (maxPriority < targetPriority)
                {
                    maxPriority = targetPriority;

                    _minionTarget = targets[i];
                }
            }

            if (_minionTarget != null && previousTarget != _minionTarget)
            {
                TargetFound?.Invoke();
            }


            return _minionTarget;
        }

        private float CalculateTargetPriority(IMinion targetMinion)
        {
            float retMinion = 0;

            int distance = CalculateDistance(_minion, targetMinion);

            float targetPriority = 0;

            switch (targetMinion.Class)
            {
                case MinionClass.Gladiator:
                    targetPriority = _minion.PriorityConfig.Gladiator;
                    break;
                case MinionClass.Templar:
                    targetPriority = _minion.PriorityConfig.Templar;
                    break;
                case MinionClass.Ranger:
                    targetPriority = _minion.PriorityConfig.Ranger;
                    break;
                case MinionClass.Assassin:
                    targetPriority = _minion.PriorityConfig.Assassin;
                    break;
                case MinionClass.Spiritmaster:
                    targetPriority = _minion.PriorityConfig.SpiritMaster;
                    break;
                case MinionClass.Cleric:
                    targetPriority = _minion.PriorityConfig.Cleric;
                    break;
                case MinionClass.Chanter:
                    targetPriority = _minion.PriorityConfig.Chanter;
                    break;
                case MinionClass.Sorcerer:
                    targetPriority = _minion.PriorityConfig.Sorcerer;
                    break;
            }

            retMinion = (1f / (distance)) + (targetPriority * (targetMinion.PriorityConfig.GoalAggression));
            retMinion += targetMinion.BonusAggression;
            
            /*
            Debug.LogError(
                          _minion.Class + "/" + _minion.Grade + "/" + _minion.Fraction + "/" + _minion.Parameters.AttackRadius.Value + " / " + retMinion
                          + " / " + targetMinion.Class + "/" + targetMinion.Grade + "/" + targetMinion.Fraction + " / this."
                          + targetPriority + " / " + distance + " / evil." + targetMinion.PriorityConfig.GoalAggression);
            */

            return retMinion;
        }

        private int CalculateDistance(IMinion minion, IMinion targetMinion)
        {
            var unitPosition = minion.Position;
            var targetPosition = targetMinion.Position;

            var distanceX = Mathf.Abs(unitPosition.x - targetPosition.x);
            var distanceY = Mathf.Abs(unitPosition.y - targetPosition.y);
            int distance = (int)Mathf.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

            if(minion.Parameters.AttackRadius.Value > 1)
                distance -= (int)minion.Parameters.AttackRadius.Value;

            distance = Mathf.Max(distance, 1);
            return distance;
        }

        private bool TryAddToPriors(IMinion target) // TODO fix N-46
        {
            if (CurrentPriorityTargets.Contains(target))
                return false;

            CurrentPriorityTargets.Add(target);
            return true;
        }

        private bool TryRemoveFromPriors(IMinion target)
        {
            return CurrentPriorityTargets.Remove(target);
        }

        public void SetMinion(IMinion minion)
        {
            _minion = minion;
        }

        public void StartBattle()
        {
            FindTargets();
        }
    }
}
