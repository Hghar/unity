using UnityEngine;

namespace Model.Maps.Types
{
    public class ShopTile : ITile
    {
        public Vector2Int Position { get; }

        public ShopTile(Vector2Int position)
        {
            Position = position;
        }
    }
}