using System.Collections.Generic;
using Fight.Targeting;
using Units;
using UnityEngine;

namespace Movement.Pathfinders
{
    public class FromEnemyPathfinder : MonoBehaviour, IPathfinder
    {
        [SerializeField] private Transform _center;
        [SerializeField] private TargetContainer _targetContainer;

        public Vector2 ComputeDirection()
        {
            IEnumerable<IMinion> targets = _targetContainer.Targets;
            Vector2 sumVectorFromEnemies = Vector2.zero;

            foreach (IMinion target in targets)
            {
                sumVectorFromEnemies += (Vector2)_center.position - target.Position;
            }

            return sumVectorFromEnemies.normalized;
        }
    }
}