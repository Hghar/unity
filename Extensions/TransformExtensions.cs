using Point;
using Units;
using Helpers.Position;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Vector2 GetPosition2D(this Transform transform)
        {
            return new Vector2(transform.position.x, transform.position.y);
        }

        public static float ComputeSqrDistanceTo(this IReadOnlyPosition point, IReadOnlyPosition other)
        {
            return (point.WorldPosition - other.WorldPosition).sqrMagnitude;
        }

        public static float ComputeSqrDistanceTo(this Transform point, IMinion other)
        {
            return (other.Position - (Vector2)point.position).sqrMagnitude;
        }

        public static float ComputeSqrDistanceTo(this IReadOnlyPosition point, Transform other)
        {
            return (point.WorldPosition - (Vector2)other.position).sqrMagnitude;
        }

        public static float ComputeSqrDistanceTo(this Transform point, Transform other)
        {
            return ((Vector2)point.position - (Vector2)other.position).sqrMagnitude;
        }
    }
}