using UnityEngine;

namespace Realization.Location
{
    public class DoorAnimator : MonoBehaviour
    {
        private const string OpenStateName = "TopDoorOpen";

        [SerializeField] private Animator _animator;

        public void Open()
        {
            _animator.Play(OpenStateName);
        }
    }
}