using UnityEngine;

namespace ZoneSize
{
    public class ZoneSize : MonoBehaviour, IZoneSize
    {
        [SerializeField] Transform _topLeftPoint;
        [SerializeField] Transform _bottomRightPoint;

        public float TopBorder => _topLeftPoint.position.y;
        public float BottomBorder => _bottomRightPoint.position.y;
        public float LeftBorder => _topLeftPoint.position.x;
        public float RightBorder => _bottomRightPoint.position.x;

        private void OnValidate()
        {
            if (_topLeftPoint != null && _bottomRightPoint != null)
            {
                if (_topLeftPoint.position.x > _bottomRightPoint.position.x)
                {
                    Debug.LogWarning("Left point is righter than right point. \nLeft point will relocate");
                    _topLeftPoint.position = new Vector2(_bottomRightPoint.position.x, _topLeftPoint.position.y);
                }

                if (_topLeftPoint.position.y < _bottomRightPoint.position.y)
                {
                    Debug.LogWarning("Top point is lower than bottom point. \nTop point will relocate");
                    _topLeftPoint.position = new Vector2(_topLeftPoint.position.x, _bottomRightPoint.position.y);
                }
            }
        }
    }
}