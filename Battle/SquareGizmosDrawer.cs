using UnityEngine;

namespace Battle
{
    public class SquareGizmosDrawer : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Color _color;
        [SerializeField] private Transform _topLeftPoint;
        [SerializeField] private Transform _bottomRightPoint;

        private void OnDrawGizmos()
        {
            Color previousColor = Gizmos.color;
            Gizmos.color = _color;

            Vector2 center = ComputeCenter(_topLeftPoint.position, _bottomRightPoint.position);
            Vector2 size = ComputeSize(_topLeftPoint.position, _bottomRightPoint.position);
            Gizmos.DrawWireCube(center, size);

            Gizmos.color = previousColor;
        }

        private Vector2 ComputeCenter(Vector2 firstPosition, Vector2 secondPosition)
        {
            return (firstPosition + secondPosition) / 2;
        }

        private Vector2 ComputeSize(Vector2 topLeftPosition, Vector2 bottomRightPosition)
        {
            return new Vector2(bottomRightPosition.x - topLeftPosition.x, topLeftPosition.y - bottomRightPosition.y);
        }
#endif
    }
}