using Fight.Healing;
using UnityEngine;

namespace Sounds
{
    public class HealingSoundPlayer : MonoBehaviour
    {
        [SerializeField] private Healer _healer;
        [SerializeField] private AudioSource _audioSource;

        private void OnEnable()
        {
            _healer.HealingStarted += OnHealingStarted;
            _healer.HealingStopped += OnHealingStopped;

            if (_healer.IsHealing)
                OnHealingStarted();
        }

        private void OnDisable()
        {
            _healer.HealingStarted -= OnHealingStarted;
            _healer.HealingStopped -= OnHealingStopped;
        }

        private void OnHealingStopped()
        {
            _audioSource.Stop();
        }

        private void OnHealingStarted()
        {
            _audioSource.Play();
        }
    }
}