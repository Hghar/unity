using Fight;
using UnityEngine;

namespace Sounds
{
    public class MinionDeathSoundPlayer : MonoBehaviour
    {
        [SerializeField] private Mortality _minion;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MinionSoundsLibrary _soundsLibrary;

        private void OnEnable()
        {
            _minion.Dying += OnMinionDying;
        }

        private void OnDisable()
        {
            _minion.Dying -= OnMinionDying;
        }

        private void OnMinionDying()
        {
            AudioClip audioClip = _soundsLibrary.GetRandomDeathSound();
            _audioSource.PlayOneShot(audioClip);
        }
    }
}