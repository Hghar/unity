using Movement.Estimators;
using UnityEngine;

namespace Movement.Pathfinders
{
    public class FightPathfinder : MonoBehaviour, IPathfinder
    {
        [SerializeField] private ToEnemyPathfinder _toEnemyPathfinder;
        [SerializeField] private FromEnemyPathfinder _fromEnemyPathfinder;
        [SerializeField] private TargetDistanceEstimator _targetDistanceEstimator;
        [SerializeField] private InZonePositionEstimator _inZonePositionEstimator;

        public Vector2 ComputeDirection()
        {
            Vector2 byTargetDirection = ComputeByTargetDirection();

            LimitedValueState xZoneEstimation = _inZonePositionEstimator.ComputeXEstimation();
            LimitedValueState yZoneEstimation = _inZonePositionEstimator.ComputeYEstimation();

            float xDirectionCoordinate = ComputeDirectionCoordinate(xZoneEstimation, byTargetDirection.x);
            float yDirectionCoordinate = ComputeDirectionCoordinate(yZoneEstimation, byTargetDirection.y);

            if (xDirectionCoordinate == 0 && yDirectionCoordinate == 0)
                return Vector2.zero;

            return new Vector2(xDirectionCoordinate, yDirectionCoordinate).normalized;
        }

        private Vector2 ComputeByTargetDirection()
        {
            if (_targetDistanceEstimator.IsTargetNear())
                return _fromEnemyPathfinder.ComputeDirection();

            if (_targetDistanceEstimator.IsTargetFar())
                return _toEnemyPathfinder.ComputeDirection();

            return Vector2.zero;
        }

        private float ComputeDirectionCoordinate(LimitedValueState estimation, float coordinate)
        {
            switch (estimation)
            {
                case LimitedValueState.OverMax:
                    coordinate = -1;
                    break;
                case LimitedValueState.MaxEdge:
                    if (coordinate > 0)
                        coordinate = 0;
                    break;
                case LimitedValueState.Middle:
                    break;
                case LimitedValueState.MinEdge:
                    if (coordinate < 0)
                        coordinate = 0;
                    break;
                case LimitedValueState.LessMin:
                    coordinate = 1;
                    break;
                default:
                    Debug.LogError($"Unexpected {nameof(LimitedValueState)}: {estimation}");
                    break;
            }

            return coordinate;
        }
    }
}