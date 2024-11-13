using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Map
{
    public class FullMapSizeFitter : MonoBehaviour
    {
        public void FitSize(GridLayoutGroup grid)
        {
            Vector2 gridPixelSize = grid.GetComponent<RectTransform>().rect.size;
            Vector2 gridVisibleSize = grid.ComputeVisibleSize();

            float occupiedXProportion = gridVisibleSize.x / gridPixelSize.x;
            float occupiedYProportion = gridVisibleSize.y / gridPixelSize.y;

            if (occupiedXProportion > occupiedYProportion)
                Fit(grid, occupiedXProportion);
            else
                Fit(grid, occupiedYProportion);
        }

        private void Fit(GridLayoutGroup grid, float occupiedProportion)
        {
            grid.cellSize /= occupiedProportion;
            grid.padding.top = (int) (grid.padding.top / occupiedProportion);
            grid.padding.right = (int) (grid.padding.right / occupiedProportion);
            grid.padding.bottom = (int) (grid.padding.bottom / occupiedProportion);
            grid.padding.left = (int) (grid.padding.left / occupiedProportion);
            grid.spacing /= occupiedProportion;
        }
    }
}