using UnityEngine;
using Zenject;

namespace Units.Ai
{
    public class ExistingMinionAi : MonoBehaviour
    {
        [SerializeField] private MinionAi _minionAi;

        [Inject]
        private void Construct(IMinionsAiPool minionsAiPool)
        {
            if (minionsAiPool.TryAdd(_minionAi) == false)
                Debug.LogError($"Failed to add {nameof(_minionAi)} to {nameof(IMinionsAiPool)} in {name}");
        }
    }
}