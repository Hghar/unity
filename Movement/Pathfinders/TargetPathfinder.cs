using Helpers.Position;
using UnityEngine;

namespace Movement.Pathfinders
{
    public class TargetPathfinder : MonoBehaviour, IPathfinder, IReminder
    {
        private IImmortalPosition _follower;
        private IImmortalPosition _target;

        public float ReminderSqr => Reminder.sqrMagnitude;

        public Vector2 Reminder => (_target == null || _follower == null) ? Vector2.zero : _target.WorldPosition - _follower.WorldPosition;

        public void Init(IImmortalPosition follower, IImmortalPosition target)
        {
            _follower = follower;
            _target = target;
        }

        public Vector2 ComputeDirection()
        {
            if (_target == null || _follower == null)
                return Vector2.zero;

            if (_target.WorldPosition == _follower.WorldPosition)
                return Vector2.zero;

            return Reminder.normalized;
        }
    }
}