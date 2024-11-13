using Units;
using UnityEngine;

namespace Sounds
{
    public class MinionArmingSoundPlayer : MonoBehaviour
    {
        [SerializeReference] private MonoBehaviour _minion;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MinionSoundsLibrary _soundsLibrary; // TODO: interface from zenject

        private IMinion Minion => _minion as IMinion;

        private void OnEnable()
        {
            // Minion.ClassChanged += OnMinionClassChanged;
        }

        private void OnDisable()
        {
            // Minion.ClassChanged -= OnMinionClassChanged;
        }

        private void OnMinionClassChanged(bool shouldAnnounce)
        {
            if (shouldAnnounce)
            {
                AudioClip audioClip = _soundsLibrary.GetStartSound(Minion.Class);
                _audioSource.clip = audioClip;
                _audioSource.Play();
            }
        }
    }
}