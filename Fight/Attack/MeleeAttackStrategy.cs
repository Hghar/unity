using Fight.Damaging;
using Fight.Fractions;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public class MeleeAttackStrategy : MonoBehaviour, IAttackStrategy
    {
        [SerializeField] private DamageDealer _prefab;
        [SerializeField] private Transform _spawnPoint;

        private IDamageDealerFactory _damageDealerFactory;

        private void Awake()
        {
            _damageDealerFactory = new DamageDealerFactory(_prefab);
        }

        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion caster)
        {
            target.Damage(damage, caster);
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