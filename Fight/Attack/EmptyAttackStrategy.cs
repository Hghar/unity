using Fight.Damaging;
using Fight.Fractions;
using Helpers.Position;
using Point;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public class EmptyAttackStrategy : MonoBehaviour, IAttackStrategy
    {
        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion minion)
        {
        }

        public void Init(Transform spawnPoint)
        {
        }

        public MonoBehaviour ToMonoBehaviour()
        {
            return this;
        }
    }
}