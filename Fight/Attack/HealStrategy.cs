using Fight.Damaging;
using Fight.Fractions;
using Units;
using UnityEngine;

namespace Fight.Attack
{
    public class HealStrategy : IAttackStrategy
    {
        private CriticalChance _criticalChance;
        private CriticalMultiplier _criticalMultiplier;

        public HealStrategy(CriticalChance criticalChance, CriticalMultiplier criticalMultiplier)
        {
            _criticalChance = criticalChance;
            _criticalMultiplier = criticalMultiplier;
        }
    
        public void Init(Transform spawnPoint)
        {
            throw new System.NotImplementedException();
        }

        public void Attack(IDamage damage, Fraction fraction, IMinion target, IMinion minion)
        {
            int _healing;
            _healing = (int)damage.Value;

            if (Random.Range(0f, 1f) < _criticalChance.Value)
            {
                _healing = (int)(damage.Value * _criticalMultiplier.Value);
            }

            if (fraction == Fraction.Enemies)
            {
                return;
            }
            Debug.Log($"{target.GameObject.name} get healing: {damage.Value}");
            target.Heal(_healing);
        }

        public MonoBehaviour ToMonoBehaviour()
        {
            throw new System.NotImplementedException();
        }
    }
}