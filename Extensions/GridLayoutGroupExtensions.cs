using System;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class GridLayoutGroupExtensions
    {
        public static Vector2 ComputeVisibleSize(this GridLayoutGroup grid)
        {
            Vector2Int tableSize = grid.ComputeTableSize();
            float visibleSizeX = grid.padding.left +
                                 grid.padding.right +
                                 tableSize.x * grid.cellSize.x +
                                 (tableSize.x - 1) * grid.spacing.x;

            float visibleSizeY = grid.padding.top +
                                 grid.padding.bottom +
                                 tableSize.y * grid.cellSize.y +
                                 (tableSize.y - 1) * grid.spacing.y;

            return new Vector2(visibleSizeX, visibleSizeY);
        }

        public static Vector2Int ComputeTableSize(this GridLayoutGroup grid)
        {
            if (grid.transform.childCount == 0)
                return Vector2Int.zero;

            switch (grid.constraint)
            {
                case GridLayoutGroup.Constraint.Flexible:
                    return ComputeFlexibleTableSize(grid);
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    return ComputeFixColumnsTableSize(grid);
                case GridLayoutGroup.Constraint.FixedRowCount:
                    return ComputeFixRowsTableSize(grid);
                default:
                    throw new InvalidOperationException(
                        $"Unexpected {nameof(GridLayoutGroup.Constraint)} in {nameof(grid)}");
            }
        }

        public static Vector2 GetRectSize(this GridLayoutGroup grid)
        {
            return grid.GetComponent<RectTransform>().rect.size;
        }

        private static Vector2Int ComputeFlexibleTableSize(GridLayoutGroup grid)
        {
            if (grid.constraint != GridLayoutGroup.Constraint.Flexible)
                throw new InvalidOperationException();

            if (grid.startAxis == GridLayoutGroup.Axis.Horizontal)
                return ComputeFlexibleHorisontalTableSize(grid);
            else
                return ComputeFlexibleVerticalTableSize(grid);
        }

        private static Vector2Int ComputeFlexibleHorisontalTableSize(GridLayoutGroup grid)
        {
            if (grid.constraint != GridLayoutGroup.Constraint.Flexible)
                throw new ArgumentException();

            if (grid.startAxis != GridLayoutGroup.Axis.Horizontal)
                throw new ArgumentException();

            ComputeFlexibleTableSize(
                grid.padding.left + grid.padding.right,
                grid.GetRectSize().x,
                grid.cellSize.x,
                grid.spacing.x,
                grid.transform.childCount,
                out int columnsAmount,
                out int rawsAmount);

            return new Vector2Int(columnsAmount, rawsAmount);
        }

        private static Vector2Int ComputeFlexibleVerticalTableSize(GridLayoutGroup grid)
        {
            if (grid.constraint != GridLayoutGroup.Constraint.Flexible)
                throw new ArgumentException();

            if (grid.startAxis != GridLayoutGroup.Axis.Vertical)
                throw new ArgumentException();

            ComputeFlexibleTableSize(
                grid.padding.top + grid.padding.bottom,
                grid.GetRectSize().y,
                grid.cellSize.y,
                grid.spacing.y,
                grid.transform.childCount,
                out int rawsAmount,
                out int columnsAmount);

            return new Vector2Int(columnsAmount, rawsAmount);
        }

        private static void ComputeFlexibleTableSize(
            float padding,
            float rectSize,
            float cellSize,
            float spacing,
            int childCount,
            out int straightSize,
            out int parallelSize)
        {
            if (childCount <= 0)
            {
                straightSize = 0;
                parallelSize = 0;
                return;
            }

            float occupiedLength = padding;
            int consideredChildren = 0;

            while (occupiedLength + cellSize <= rectSize &&
                   consideredChildren < childCount)
            {
                occupiedLength += cellSize + spacing;
                consideredChildren++;
            }

            straightSize = 1;

            if (consideredChildren > 1)
                straightSize = consideredChildren;

            parallelSize = ComputeParallelSize(childCount, straightSize);
        }

        private static Vector2Int ComputeFixColumnsTableSize(GridLayoutGroup grid)
        {
            if (grid.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
                throw new ArgumentException();

            int columnsAmount = grid.constraintCount;
            int rawsAmount = ComputeParallelSize(grid.transform.childCount, columnsAmount);
            return new Vector2Int(columnsAmount, rawsAmount);
        }

        private static Vector2Int ComputeFixRowsTableSize(GridLayoutGroup grid)
        {
            if (grid.constraint != GridLayoutGroup.Constraint.FixedRowCount)
                throw new ArgumentException();

            int rawsAmount = grid.constraintCount;
            int columnsAmount = ComputeParallelSize(grid.transform.childCount, rawsAmount);
            return new Vector2Int(columnsAmount, rawsAmount);
        }

        private static int ComputeParallelSize(int childCount, int straightSize)
        {
            return Mathf.CeilToInt((float) childCount / straightSize);
        }
    }
}