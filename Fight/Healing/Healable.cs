using Fight.Fractions;
using UnityEngine;

namespace Fight.Healing
{
    public class Healable : MonoBehaviour, IHealable
    {
        [SerializeField] private FractionMarker _fractionMarker;
        [SerializeField] private Health _health;

        public Fraction Fraction => _fractionMarker.Fraction;

        public void Heal(int healingValue)
        {
            _health.Increase(healingValue);
        }
    }
}