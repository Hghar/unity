using Fight.Damaging;
using Fight.Fractions;
using Helpers.Position;
using UnityEngine;

namespace Fight.Attack.Projectiles
{
    public interface IProjectileFactory
    {
        public void Create(IDamage damage, Fraction fraction, IDestroyablePoint target, Vector2 position);
    }
}