using Fight.Attack;
using Fight.Targeting;
using Parameters;
using UnityEngine;

namespace Movement.Estimators
{
    public class TargetDistanceEstimator : MonoBehaviour
    {
        [SerializeField] private Transform _center;
        [SerializeField] private FightTargetFinder _targetFinder;

        private AttackRadius _attackRadius;

        private RetreatTriggerRadius _retreatTriggerRadus;

        private float _nearEvaluatingSqrDistance;

        private float _farEvaluatingSqrDistance;

        public FightTargetFinder TargetFinder => _targetFinder;

        public void SetConfig(IUnitParameters parameters)
        {
            _attackRadius = parameters.AttackRadius;
            _retreatTriggerRadus = new RetreatTriggerRadius(1);
            // TODO: subscribe on the attack radius changes
            _nearEvaluatingSqrDistance = _retreatTriggerRadus.Value * _retreatTriggerRadus.Value;
            _farEvaluatingSqrDistance = _attackRadius.Value * _attackRadius.Value;
        }

        public void UpdateConfig(IUnitParameters parameters)
        {
            _attackRadius = parameters.AttackRadius;
            _retreatTriggerRadus = new RetreatTriggerRadius(1);
            // TODO: subscribe on the attack radius changes
            _nearEvaluatingSqrDistance = _retreatTriggerRadus.Value * _retreatTriggerRadus.Value;
            _farEvaluatingSqrDistance = _attackRadius.Value * _attackRadius.Value;
        }

        public bool IsTargetNear()
        {
            // TODO: subscribe on the attack radius changings
            _nearEvaluatingSqrDistance = _retreatTriggerRadus.Value * _retreatTriggerRadus.Value;

            if (TargetFinder.TryComputeSqrDistanceToTarget(out float distanceToTarget) == false)
                return false;

            return distanceToTarget <= _nearEvaluatingSqrDistance;
        }

        public bool IsTargetFar()
        {
            // TODO: subscribe on the attack radius changings
            _farEvaluatingSqrDistance = _attackRadius.Value * _attackRadius.Value;

            if (TargetFinder.TryComputeSqrDistanceToTarget(out float distanceToTarget) == false)
                return false;

            return distanceToTarget >= _farEvaluatingSqrDistance;
        }
    }
}