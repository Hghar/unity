using UnityEngine;

namespace Infrastructure.Shared.Extensions
{
    public static class Vector2IntExtensions
    {
        public static bool IsConnectedTo(this Vector2Int position, Vector2Int to)
        {
            int xDiff = Mathf.Abs(position.x - to.x);
            int yDiff = Mathf.Abs(position.y - to.y);

            if (xDiff <= 1 &&
                yDiff <= 1 &&
                xDiff - yDiff != 0)
                return true;

            return false;
        }
    }
}