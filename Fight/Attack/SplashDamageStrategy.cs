using System.Collections.Generic;
using System.Linq;
using Fight.Damaging;
using Fight.Fractions;
using Fight.Targeting;
using Grids;
using Units;
using UnityEngine;
using Zenject;

namespace Fight.Attack
{
    public class SplashDamageStrategy : MonoBehaviour, IAttackStrategy
    {
        [SerializeField] private DamageDealer _prefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private FightTargetFinder _fightTargetFinder;

        private IDamageDealerFactory _damageDealerFactory;
        private IGrid<IMinion> _grid;
        private Gladiator _minion;

        [Inject]
        private void Construct(IGrid<IMinion> grid)
        {
            _grid = grid;
        }

        private void Awake()
        {
            _damageDealerFactory = new DamageDealerFactory(_prefab);
            _minion = gameObject.GetComponentInParent<Gladiator>();
        }

        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion caster)
        {
            var targetCoordinates = target.Position;
            var unitCoordinates = _minion.Position;

            var colliders =
                Physics2D.OverlapCircleAll(target.GameObject.transform.position, 10);
            var splashDamage = new Damage(Mathf.RoundToInt(damage.Value * _minion.ClassParameters.AoeDamage));

            var minions = new List<IMinion>();
            foreach (var collider in colliders)
            {
                var minion = collider.GetComponentInParent<IMinion>();
                if (minion != null && minion.Fraction != _minion.Fraction && minions.Contains(minion) == false)
                {
                    minions.Add(minion);
                }
            }

            DamageNeighborhoods(
                minions,
                targetCoordinates - unitCoordinates,
                splashDamage,
                targetCoordinates
            );
            target.Damage(damage, caster);
        }

        private void DamageNeighborhoods(List<IMinion> minions, Vector2 difference, IDamage damage, Vector2 targetPosition)
        {
            var minionsToDamage = new List<IMinion>();
            
            if (difference.x == 0)
            {
                var minion = minions.FirstOrDefault((m => m.Position == targetPosition+Vector2.left));
                var minion1 = minions.FirstOrDefault((m => m.Position == targetPosition+Vector2.right));
                if(minion != null)
                    minionsToDamage.Add(minion);
                if(minion1 != null)
                    minionsToDamage.Add(minion1);
            }
            if (difference.y == 0)
            {
                var minion = minions.FirstOrDefault((m => m.Position == targetPosition+Vector2.up));
                var minion1 = minions.FirstOrDefault((m => m.Position == targetPosition+Vector2.down));
                if(minion != null)
                    minionsToDamage.Add(minion);
                if(minion1 != null)
                    minionsToDamage.Add(minion1);
            }
            if (Mathf.Abs(difference.x) >= 1 && Mathf.Abs(difference.y) >= 1)
            {
                var minion = minions.FirstOrDefault((m => m.Position == targetPosition-new Vector2(difference.x, 0)));
                var minion1 = minions.FirstOrDefault((m => m.Position == targetPosition-new Vector2(0, difference.y)));
                if(minion != null)
                    minionsToDamage.Add(minion);
                if(minion1 != null)
                    minionsToDamage.Add(minion1);
            }
            
            foreach (var minion in minionsToDamage)
            {
                minion.Damage(damage, _minion);
            }
        }

        public void Init(Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        public MonoBehaviour ToMonoBehaviour()
        {
            return this;
        }
    }
}