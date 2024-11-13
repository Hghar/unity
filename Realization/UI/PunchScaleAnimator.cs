using DG.Tweening;
using UnityEngine;

namespace Realization.UI
{
    public class PunchScaleAnimator : MonoBehaviour
    {
        [SerializeField] private float _force = 0.5f;
        [SerializeField] private int _vibrato = 5;
        [SerializeField] private float _duration = 1;
        [SerializeField] private float _delay = 1;

        private void Awake()
        {
            //Animation();
        }

        private void Animation()
        {
            Vector3 startScale = this.transform.localScale;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOPunchScale(startScale * _force, _duration, _vibrato));
            sequence.AppendInterval(_delay);
            sequence.SetLoops(-1);
            sequence.Play();
        }
    }
}