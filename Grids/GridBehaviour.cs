using System.Collections.Generic;
using UnityEngine;

namespace Grids
{
    public class GridBehaviour : MonoBehaviour
    {
        public CellBehaviour CellPrefab;
        public float CellOffset;
        public Dictionary<Vector2Int, CellBehaviour> Cells = new();

        public void InitCells(Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var cell = Instantiate(CellPrefab, transform);
                    cell.transform.localPosition = new Vector3(x, -y, 0) * cell.Size;
                    cell.name = $"Cell_{x + 1}_{y + 1}";
                    if (x != 0)
                        cell.transform.localPosition += new Vector3(CellOffset * x, 0, 0);
                    if (y != 0)
                        cell.transform.localPosition += new Vector3(0, -CellOffset * y, 0);
                    Cells.Add(new Vector2Int(x, y), cell);
                }
            }

            Vector3 size3 = new Vector3(size.x, -size.y, 0);
            transform.localPosition = -(size3 * CellPrefab.Size);
            transform.localPosition -= new Vector3(CellOffset * size3.x, CellOffset * size3.y, 0);
            transform.localPosition /= 2;
            transform.localPosition += new Vector3(CellPrefab.Size, -CellPrefab.Size);
            transform.localPosition -= new Vector3(CellOffset, -CellOffset, 0) / 2;
        }

        public Vector3 ToWorld(Vector2Int position)
        {
            return Cells[position].transform.position;
        }

        public Vector2Int FindClosest(Vector2 vector2)
        {
            float maxDistance = float.MaxValue;
            Vector2Int? closestPosition = null;
            
            foreach (var cell in Cells)
            {
                var distance = Vector3.Distance(vector2, cell.Value.transform.position);
                if (distance <= maxDistance)
                {
                    maxDistance = distance;
                    closestPosition = cell.Key;
                }
            }

            if (closestPosition == null)
            {
                Debug.LogError($"There is no cell for position {vector2}");
                return default;
            }

            return (Vector2Int)closestPosition;
        }
    }
}