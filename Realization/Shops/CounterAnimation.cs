using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.Shops
{
    public class CounterAnimation:MonoBehaviour
    {
        private const float Duration = 0.6f;
        public Transform Parent;
        public CanvasGroup CanvasGroup;
        public Transform Start;
        public Transform End;
        
        public void Play()
        {
            Sequence sequence = DOTween.Sequence();
            CanvasGroup.gameObject.SetActive(true);
            CanvasGroup.transform.position = Start.position;
            CanvasGroup.alpha = 0.1f;
            sequence.Join(CanvasGroup.transform.DOMove(End.position, Duration));
            sequence.Join(CanvasGroup.DOFade(1, Duration));
            sequence.OnComplete(Disable);
            sequence.Play();
        }

        private void Disable()
        {
            CanvasGroup.gameObject.SetActive(false);
        }
    }
}