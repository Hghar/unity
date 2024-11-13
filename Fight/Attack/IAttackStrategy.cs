using Fight.Damaging;
using Fight.Fractions;
using Point;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public interface IAttackStrategy
    {
        public void Init(Transform spawnPoint);
        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion caster);

        public MonoBehaviour ToMonoBehaviour();
    }
}