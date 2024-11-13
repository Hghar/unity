using UnityEngine;

namespace Movement.Pathfinders
{
    public interface IPathfinder
    {
        public Vector2 ComputeDirection();
    }
}