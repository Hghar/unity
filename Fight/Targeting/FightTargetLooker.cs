using UnityEngine;

namespace Fight.Targeting
{
    public class FightTargetLooker : MonoBehaviour
    {
        [SerializeField] private FightTargetFinder _targetFinder;

        private void Update()
        {
            if (_targetFinder.TryGetTargetPosition(out Vector2 targetPosition))
                transform.rotation =
                    Quaternion.LookRotation(Vector3.forward, targetPosition - (Vector2) transform.position);
        }
    }
}