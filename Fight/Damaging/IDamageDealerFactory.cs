using Fight.Fractions;
using UnityEngine;

namespace Fight.Damaging
{
    public interface IDamageDealerFactory
    {
        public void Create(IDamage damage, Fraction fraction, Vector2 position);
    }
}