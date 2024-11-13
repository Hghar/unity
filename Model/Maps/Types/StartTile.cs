using UnityEngine;

namespace Model.Maps.Types
{
    public class StartTile : ITile
    {
        public Vector2Int Position { get; }

        public StartTile(Vector2Int position)
        {
            Position = position;
        }
    }
}