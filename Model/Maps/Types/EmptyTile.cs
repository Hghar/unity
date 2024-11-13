using UnityEngine;

namespace Model.Maps.Types
{
    public class EmptyTile : ITile
    {
        public Vector2Int Position { get; }

        public EmptyTile(Vector2Int position)
        {
            Position = position;
        }
    }
}