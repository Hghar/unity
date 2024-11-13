using UnityEngine;

namespace Fight.Fractions
{
    public class FractionMarker : MonoBehaviour, IFractionMarker
    {
        [SerializeField] private Fraction _fraction;

        public Fraction Fraction => _fraction;
    }
}