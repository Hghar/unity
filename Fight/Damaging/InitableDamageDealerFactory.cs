using System;
using Fight.Fractions;
using UnityEngine;

namespace Fight.Damaging
{
    [Serializable]
    public class InitableDamageDealerFactory // TODO : resolve necessity to use IDamageDealerFactory
    {
        [SerializeField] private DamageDealer _damageDealerPrefab;

        private Transform _transform;
        private IDamageDealerFactory _damageDealerFactory;
        private IDamage _damage;
        private Fraction _fraction;

        public Fraction Fraction => _fraction;

        public void Create()
        {
            _damageDealerFactory.Create(_damage, _fraction, _transform.position);
        }

        public void Init(IDamage damage, Fraction fraction, Transform transform)
        {
            _damageDealerFactory = new DamageDealerFactory(_damageDealerPrefab);
            _damage = damage;
            _fraction = fraction;
            _transform = transform;
        }
    }
}