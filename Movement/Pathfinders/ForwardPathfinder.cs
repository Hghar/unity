using UnityEngine;

namespace Movement.Pathfinders
{
    public class ForwardPathfinder : MonoBehaviour, IPathfinder
    {
        public Vector2 ComputeDirection()
        {
            return Vector2.right;
        }
    }
}