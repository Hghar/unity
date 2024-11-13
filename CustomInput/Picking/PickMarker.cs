using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomInput.Picking
{
    public class PickMarker : MonoBehaviour
    {
        private const float PivotCoordinateProportion = 0.5f;
        private const int SizeMeasuresFromZeroBonus = 1;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _cellSize = 1.5f;
        [SerializeField] private float _padding = 0f;

        private void OnValidate()
        {
            if (_spriteRenderer.drawMode != SpriteDrawMode.Sliced)
                Debug.LogWarning($"drawMode of {nameof(PickMarker)}'s renderer " +
                                 $"in {gameObject.name} should be {SpriteDrawMode.Sliced}");
        }

        public void Init(IEnumerable<Vector2Int> shape)
        {
            int maxX = shape.Max(point => point.x);
            int minX = shape.Min(point => point.x);
            int maxY = shape.Max(point => point.y);
            int minY = shape.Min(point => point.y);

            ComputeSize(maxX, minX, maxY, minY);
            ComputePosition(maxX, minX, maxY, minY);
        }

        public void SwitchOn()
        {
            _spriteRenderer.enabled = true;
        }

        public void SwitchOff()
        {
            if (_spriteRenderer != null)
                _spriteRenderer.enabled = false;
        }

        private void ComputeSize(int maxX, int minX, int maxY, int minY)
        {
            float xSize = ComputeSizeCoordinate(maxX, minX);
            float ySize = ComputeSizeCoordinate(maxY, minY);
            _spriteRenderer.size = new Vector2(xSize, ySize);
        }

        private float ComputeSizeCoordinate(int max, int min)
        {
            return (max - min + SizeMeasuresFromZeroBonus) * _cellSize + _padding + _padding;
        }

        private void ComputePosition(int maxX, int minX, int maxY, int minY)
        {
            float positionX = ComputePositionCoordinate(maxX, minX);
            float positionY = ComputePositionCoordinate(maxY, minY);
            transform.localPosition = new Vector2(positionX, -positionY);
        }

        private float ComputePositionCoordinate(int max, int min)
        {
            return (max + min) * _cellSize * PivotCoordinateProportion;
        }
    }
}