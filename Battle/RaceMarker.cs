using UnityEngine;

namespace Battle
{
    public class RaceMarker : MonoBehaviour
    {
        [SerializeField] private Race _race;

        public Race Race => _race;
    }
}