using System.Collections;
using Fight.Damaging;
using Fight.Fractions;
using Movement.Pathfinders;
using Helpers.Position;
using UnityEngine;

namespace Fight.Attack.Projectiles
{
    [RequireComponent(typeof(DestroyablePoint))]
    public class TargetedProjectile : MonoBehaviour
    {
        [SerializeField] private InitableDamageDealerFactory _damageDealerFactory;
        [SerializeField] private TargetPathfinder _targetPathfinder;
        [SerializeField] [Min(0)] private float _minimalHitDistance = 0.1f;

        private UnmovableAfterDestroyingPoint _thisPoint;
        private UnmovableAfterDestroyingPoint _target;

        private void Awake()
        {
            DestroyablePoint destroyablePoint = GetComponent<DestroyablePoint>();
            _thisPoint = new UnmovableAfterDestroyingPoint(destroyablePoint);
        }

        private IEnumerator Start()
        {
            yield return WaitForTargetApproach();
            Hit();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDestroyablePoint collisionDestroyablePoint))
            {
                if (_target.HasPoint(collisionDestroyablePoint))
                {
                    Hit();
                }
            }
        }

        private void OnDestroy()
        {
            _thisPoint.Dispose();
            _target.Dispose();
        }

        public void Init(IDamage damage, Fraction fraction, IDestroyablePoint target)
        {
            _target = new UnmovableAfterDestroyingPoint(target);
            _targetPathfinder.Init(_thisPoint, _target);
            _damageDealerFactory.Init(damage, fraction, transform);
        }

        private IEnumerator WaitForTargetApproach()
        {
            float minimalHitDistanceSqr = _minimalHitDistance * _minimalHitDistance;
            yield return new WaitUntil(() =>
                ((Vector2) transform.position - _target.WorldPosition).sqrMagnitude <= minimalHitDistanceSqr);
        }

        private void Hit()
        {
            _damageDealerFactory.Create();
            Destroy(gameObject);
        }
    }
}