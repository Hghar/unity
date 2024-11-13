using UnityEngine;

namespace Grids
{
    public interface IGridObject
    {
        string Name { get; }
        Vector2Int Position { get; set; }
    }
}