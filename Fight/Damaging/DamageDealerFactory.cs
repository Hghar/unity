using Fight.Fractions;
using UnityEngine;

namespace Fight.Damaging
{
    public class DamageDealerFactory : IDamageDealerFactory
    {
        private readonly DamageDealer _prefab;

        public DamageDealerFactory(DamageDealer prefab)
        {
            _prefab = prefab;
        }

        public void Create(IDamage damage, Fraction fraction, Vector2 position)
        {
            DamageDealer damageDealer = Object.Instantiate(_prefab, position, Quaternion.identity);
            damageDealer.Init(damage, fraction);
        }
    }
}