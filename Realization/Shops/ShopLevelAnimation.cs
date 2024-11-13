using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Realization.Shops
{
    public class ShopLevelAnimation : MonoBehaviour
    {
        private void Awake()
        {
            ColorUtility.TryParseHtmlString( "#a6e740" , out Color green );
            _greenColor = green;
        }

        public void PlayAnimation(int level)
        {
            Text.text = $"+ {level}";
            Text.color = _greenColor;

            Sequence sequence = DOTween.Sequence();
            Text.gameObject.SetActive(true);
            Text.alpha = 0.1f;
            sequence.Join(Text.DOFade(1, Duration));
            sequence.OnComplete(Disable);
            sequence.Play();
        }

        public TextMeshProUGUI Text;
        private Color _greenColor;

        private void Disable()
        {
            Text.gameObject.SetActive(false);
        }

        private const float Duration = 0.8f;
    }
}