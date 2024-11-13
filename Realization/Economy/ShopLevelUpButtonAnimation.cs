using DG.Tweening;
using Model.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Realization.Economy
{
    public class ShopLevelUpButtonAnimation : MonoBehaviour
    {
        public CanvasGroup CanvasGroup;
        public Image Arrow;
        private Color _greenColor;

        public void Awake()
        {
            ColorUtility.TryParseHtmlString("#a6e740", out Color green);
            _greenColor = green;
            Disable();
        }

        public void Show()
        {
            Sequence sequence = DOTween.Sequence();
            CanvasGroup.gameObject.SetActive(true);
            CanvasGroup.alpha = 0.1f;
            CanvasGroup.transform.localPosition  = new Vector3( CanvasGroup.transform.localPosition.x,-30,CanvasGroup.transform.localPosition.z);
            CanvasGroup.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
            sequence.Join(CanvasGroup.DOFade(0.6f, Duration));
            sequence.Join(CanvasGroup.transform.DOScale(1, Duration));
            sequence.Join(CanvasGroup.transform.DOLocalMoveY(7, Duration));
            
            sequence.Append(CanvasGroup.DOFade(0f, Duration));
            sequence.OnComplete(() =>
            {
                CanvasGroup.gameObject.SetActive(false);
            });
            
            sequence.Play();
        }

        public void Disable()
        {
            
        }

        private const float Duration = 0.5f;
    }
}