using UnityEngine;

namespace Model.Maps.Types
{
    public class BossTile : ITile
    {
        public Vector2Int Position { get; }

        public BossTile(Vector2Int position)
        {
            Position = position;
        }
    }
}