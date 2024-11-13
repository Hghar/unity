using System.Threading.Tasks;
using UnityEngine;

namespace Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        private const int MillisecondsInSecond = 1000;

        [SerializeField] private AudioClip _opening;
        [SerializeField] private AudioClip _background;

        private static BackgroundMusicPlayer _instance;
        private AudioSource _audioSource;

        public static BackgroundMusicPlayer Instance => _instance;

        private void Awake()
        {
            if (Instance == null)
                _instance = this;
            else
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource.enabled)
            {
                _audioSource.PlayOneShot(_opening);
                PlayBackground();
            }
        }

        private async void PlayBackground()
        {
            await Task.Delay((int) (_opening.length * MillisecondsInSecond));
            _audioSource.clip = _background;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }
}