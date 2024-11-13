using Model.Maps;
using Units.Ai;
using UnityEngine;
using Zenject;

namespace Sounds
{
    public class StepsSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private IMap _map;
        private IMinionsSetPositionsPublisher _minionsAiPool;

        [Inject]
        private void Construct(IMap map, IMinionsSetPositionsPublisher minionsAiPool)
        {
            _map = map;
            _minionsAiPool = minionsAiPool;
        }

        private void OnEnable()
        {
            _map.Moved += OnMinionsMoved;
            _minionsAiPool.AllMinionsSetPositions += OnMinionsSetPositions;
        }

        private void OnDisable()
        {
            _map.Moved -= OnMinionsMoved;
            _minionsAiPool.AllMinionsSetPositions -= OnMinionsSetPositions;
        }

        private void OnMinionsSetPositions()
        {
            if (_audioSource.enabled)
                _audioSource.Stop();
        }

        private void OnMinionsMoved()
        {
            if (_audioSource.enabled)
                _audioSource.Play();
        }
    }
}