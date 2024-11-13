using UnityEngine;
using Zenject;
using ZoneSize;

namespace Movement.Estimators
{
    public class InZonePositionEstimator : MonoBehaviour
    {
        [SerializeField] private Transform _center;
        [SerializeField] [Min(0)] private float _edgeWidth;

        private IZoneSize _zoneSize;

        [Inject]
        private void Construct(IZoneSize zoneSize)
        {
            _zoneSize = zoneSize;
        }

        public LimitedValueState ComputeXEstimation()
        {
            return Estimate(_center.position.x, _zoneSize.LeftBorder, _zoneSize.RightBorder);
        }

        public LimitedValueState ComputeYEstimation()
        {
            return Estimate(_center.position.y, _zoneSize.BottomBorder, _zoneSize.TopBorder);
        }

        private LimitedValueState Estimate(float coordinate, float min, float max)
        {
            if (coordinate > max)
                return LimitedValueState.OverMax;
            else if (coordinate < min)
                return LimitedValueState.LessMin;
            else if (coordinate > (max - _edgeWidth))
                return LimitedValueState.MaxEdge;
            else if (coordinate < (min + _edgeWidth))
                return LimitedValueState.MinEdge;
            else
                return LimitedValueState.Middle;
        }
    }
}