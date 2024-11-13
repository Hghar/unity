using System.Threading.Tasks;
using Battle;
using UnityEngine;
using Zenject;

namespace Sounds
{
    public class EnemySoundPlayer : MonoBehaviour
    {
        [SerializeField] private RaceMarker _raceMarker;
        [SerializeField] private EnemySoundsLibrary _soundsLibrary;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _randomDelay = 0.5f;

        private IBattleStartPublisher _battleStarter;

        [Inject]
        private void Construct(IBattleStartPublisher battleStarter)
        {
            _battleStarter = battleStarter;
        }

        private void OnEnable()
        {
            _battleStarter.BattleStarted += OnBattleStarted;
        }

        private void OnDisable()
        {
            _battleStarter.BattleStarted -= OnBattleStarted;
        }

        private void OnBattleStarted()
        {
            PlaySound();
        }

        async private void PlaySound()
        {
            await Task.Delay((int) (Random.Range(0f, _randomDelay) * 1000));
            AudioClip audioClip = _soundsLibrary.GetStartFightSound(_raceMarker.Race);
            _audioSource.PlayOneShot(audioClip);
        }
    }
}