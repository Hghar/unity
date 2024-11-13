using UnityEngine;

namespace Model.Maps.Types
{
    public interface ITile
    {
        Vector2Int Position { get; }
    }
}