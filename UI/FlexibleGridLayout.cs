using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    // TODO: develop custom editor, add using alignment
    public class FlexibleGridLayout : LayoutGroup
    {
        // TODO: add setting horisontal/vertical/equal grow direction

        [SerializeField] private Vector2 _spacing;
        [SerializeField] private GridFitType _fitType;

        [SerializeField] [Min(1)] private int _rows;
        [SerializeField] [Min(1)] private int _columns;

        [SerializeField] private bool _fitX = true;
        [SerializeField] private bool _fitY = true;

        [SerializeField] [Min(0)] private float _cellWidth;
        [SerializeField] [Min(0)] private float _cellHeight;

        public GridFitType FitType => _fitType;
        public bool FitX => _fitX;
        public bool FitY => _fitY;

        //protected override void OnValidate() uncomment when editor will being developed.
        //{
        //    if (_spacing.x < 0)
        //        _spacing = new Vector2(0, _spacing.y);

        //    if (_spacing.y < 0)
        //        _spacing = new Vector2(_spacing.x, 0);

        //    if (_rows < 1)
        //        _rows = 1;

        //    if (_columns < 1)
        //        _columns = 1;
        //}

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            CalculateRowsAndColumnsAmount();
            CalculateCellSize();
            LocateCells();
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }

        private void CalculateRowsAndColumnsAmount()
        {
            if (_fitType == GridFitType.Width || _fitType == GridFitType.Height || _fitType == GridFitType.Square)
            {
                float squareSize = Mathf.Sqrt(transform.childCount);
                _rows = Mathf.CeilToInt(squareSize);
                _columns = Mathf.CeilToInt(squareSize);
            }

            if (_fitType == GridFitType.Width || _fitType == GridFitType.FixedColumns)
            {
                if (_columns == 0)
                    return;

                _rows = Mathf.CeilToInt((float) transform.childCount / _columns);
            }
            else if (_fitType == GridFitType.Height || _fitType == GridFitType.FixedRows)
            {
                if (_rows == 0)
                    return;

                _columns = Mathf.CeilToInt((float) transform.childCount / _rows);
            }
        }

        private void CalculateCellSize()
        {
            if (_columns == 0 || _rows == 0)
                return;

            if (_fitX)
            {
                _cellWidth = rectTransform.rect.width / _columns;
                _cellWidth = ApplyPaddingToWidth(_cellWidth);
                _cellWidth = ApplySpacingToWidth(_cellWidth);
            }

            if (_fitY)
            {
                _cellHeight = rectTransform.rect.height / _rows;
                _cellHeight = ApplyPaddingToHeight(_cellHeight);
                _cellHeight = ApplySpacingToHeight(_cellHeight);
            }
        }

        private float ApplySpacingToWidth(float cellWidth)
        {
            if (_columns == 0)
                return 0;

            return cellWidth - ((_spacing.x / _columns) * (_columns - 1));
        }

        private float ApplySpacingToHeight(float cellHeight)
        {
            if (_rows == 0)
                return 0;

            return cellHeight - ((_spacing.y / _rows) * (_rows - 1));
        }

        private float ApplyPaddingToWidth(float cellWidth)
        {
            if (_columns == 0)
                return 0;

            return cellWidth - (padding.left / _columns) - (padding.right / _columns);
        }

        private float ApplyPaddingToHeight(float cellHeight)
        {
            if (_rows == 0)
                return 0;

            return cellHeight - (padding.top / _rows) - (padding.bottom / _rows);
        }

        private void LocateCells()
        {
            if (_columns == 0)
                return;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                int effectiveRowIndex = i / _columns;
                int effectiveColumnIndex = i % _columns;

                float effectivePositionX = _cellWidth * effectiveColumnIndex;
                float effectivePositionY = _cellHeight * effectiveRowIndex;

                effectivePositionX = ApplySpacingToPositionX(effectivePositionX, effectiveColumnIndex);
                effectivePositionX = ApplyPaddingToPositionX(effectivePositionX);

                effectivePositionY = ApplySpacingToPositionY(effectivePositionY, effectiveRowIndex);
                effectivePositionY = ApplyPaddingToPositionY(effectivePositionY);

                Vector2 effectivePosition = new Vector2(effectivePositionX, effectivePositionY);
                RectTransform cell = rectChildren[i];
                LocateCellAlongAxis(cell, effectivePosition, i);
            }
        }

        private float ApplySpacingToPositionX(float positionX, int columnIndex)
        {
            return positionX + (_spacing.x * columnIndex);
        }

        private float ApplySpacingToPositionY(float positionY, int rowIndex)
        {
            return positionY + (_spacing.y * rowIndex);
        }

        private float ApplyPaddingToPositionX(float positionX)
        {
            switch (childAlignment)
            {
                default:
                case TextAnchor.UpperLeft:
                case TextAnchor.LowerLeft:
                case TextAnchor.MiddleLeft:
                case TextAnchor.UpperCenter:
                    return positionX + padding.left;
                case TextAnchor.UpperRight:
                case TextAnchor.LowerRight:
                case TextAnchor.MiddleRight:
                case TextAnchor.LowerCenter:
                    return positionX + padding.right;

                case TextAnchor.MiddleCenter:

                    return positionX + padding.right - padding.left;
            }
        }

        private float ApplyPaddingToPositionY(float positionY)
        {
            switch (childAlignment)
            {
                default:
                case TextAnchor.UpperLeft:
                case TextAnchor.UpperRight:
                case TextAnchor.UpperCenter:
                    return positionY + padding.top;
                case TextAnchor.LowerLeft:
                case TextAnchor.LowerRight:
                case TextAnchor.LowerCenter:
                    return positionY + padding.bottom;
                case TextAnchor.MiddleLeft:
                case TextAnchor.MiddleRight:
                case TextAnchor.MiddleCenter:
                    return positionY + padding.top - padding.bottom;
            }
        }

        private void LocateCellAlongAxis(RectTransform cell, Vector2 effectivePosition, int cellIndex)
        {
            // TODO: Separate the grow direction setting and the middling setting
            // TODO: Handle Middle-Center, MiddleLeft, MiddleRight alignmet

            float positionX = effectivePosition.x;
            float positionY = effectivePosition.y;

            int shiftedCellsAmount = transform.childCount % _columns;
            int unshiftedCellsAmount = transform.childCount - shiftedCellsAmount;


            switch (childAlignment)
            {
                default:
                case TextAnchor.UpperLeft:
                    break;
                case TextAnchor.UpperRight:
                    positionX = rectTransform.rect.width - positionX - _cellWidth;
                    break;
                case TextAnchor.LowerLeft:
                    positionY = rectTransform.rect.height - positionY - _cellHeight;
                    break;
                case TextAnchor.LowerRight:
                    positionX = rectTransform.rect.width - positionX - _cellWidth;
                    positionY = rectTransform.rect.height - positionY - _cellHeight;
                    break;
                case TextAnchor.UpperCenter:
                    if (unshiftedCellsAmount <= cellIndex)
                        positionX += (_columns - shiftedCellsAmount) * _cellWidth / 2;

                    break;
                case TextAnchor.LowerCenter:
                    positionX = rectTransform.rect.width - positionX - _cellWidth;
                    if (unshiftedCellsAmount <= cellIndex)
                        positionX -= (_columns - shiftedCellsAmount) * _cellWidth / 2;

                    positionY = rectTransform.rect.height - positionY - _cellHeight;
                    break;
            }

            SetChildAlongAxis(cell, 0, positionX, _cellWidth);
            SetChildAlongAxis(cell, 1, positionY, _cellHeight);
        }
    }

    public enum GridFitType
    {
        Square,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }
}