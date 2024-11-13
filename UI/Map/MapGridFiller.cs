using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Map
{
    public class MapGridFiller : MonoBehaviour
    {
        [SerializeField] private RectTransform _emptySpacePrefab;

        private GridLayoutGroup _grid;
        private IEnumerable<IMapTileViewInfo> _tiles;
        private IMapTileViewFactory _factory;
        private Vector2Int? _gridSize;

        [Inject]
        private void Construct(IMapTileViewFactory factory)
        {
            _factory = factory;
        }

        public void FillGrid(IEnumerable<IMapTileViewInfo> tiles, GridLayoutGroup grid)
        {
            _tiles = tiles;
            _grid = grid;
            PrepareGridProperties();
            ClearGrid();

            if (tiles.Count() == 0)
                return;

            ComputeBorders(out int leftBorder, out int rightBorder, out int topBorder, out int bottomBorder);
            ComputeGridSize(leftBorder, rightBorder, topBorder, bottomBorder);
            ComputeConstraintCount();
            InstatieateCells(leftBorder, rightBorder, topBorder, bottomBorder);
        }

        public bool TryGetGridSize(out Vector2Int gridSize)
        {
            gridSize = Vector2Int.zero;

            if (_gridSize == null)
                return false;

            gridSize = _gridSize.Value;
            return true;
        }

        private void ComputeGridSize(int leftBorder, int rightBorder, int topBorder, int bottomBorder)
        {
            int columnsAmount = rightBorder - leftBorder + 1;
            int rawsAmount = bottomBorder - topBorder + 1;
            _gridSize = new Vector2Int(columnsAmount, rawsAmount);
        }

        private void ClearGrid()
        {
            for (int i = 0; i < _grid.transform.childCount; i++)
            {
                Transform child = _grid.transform.GetChild(i);
                Destroy(child.gameObject);
            }

            _gridSize = null;
        }

        private void ComputeBorders(out int leftBorder, out int rightBorder, out int topBorder, out int bottomBorder)
        {
            IEnumerable<int> xs = _tiles.Select(tile => tile.Position.x);
            leftBorder = xs.Min();
            rightBorder = xs.Max();

            IEnumerable<int> ys = _tiles.Select(tile => tile.Position.y);
            topBorder = ys.Min();
            bottomBorder = ys.Max();
        }

        private void ComputeConstraintCount()
        {
            _grid.constraintCount = _gridSize.Value.x;
        }

        private void InstatieateCells(int leftBorder, int rightBorder, int topBorder, int bottomBorder)
        {
            for (int column = leftBorder; column <= rightBorder; column++)
            {
                for (int row = topBorder; row <= bottomBorder; row++)
                {
                    Vector2Int gridPosition = new Vector2Int(column, row);
                    IMapTileViewInfo tileViewInfo = _tiles.FirstOrDefault(tile => tile.Position == gridPosition);

                    if (tileViewInfo != null)
                        _factory.Create(_grid.transform, tileViewInfo);
                    else
                        Instantiate(_emptySpacePrefab, _grid.transform);
                }
            }
        }

        private void PrepareGridProperties()
        {
            _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
            _grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        }
    }
}