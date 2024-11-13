using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Realization.Shops
{
    public class GradeAnimation : MonoBehaviour
    {
        private void Awake()
        {
            ColorUtility.TryParseHtmlString(  "#a6e740", out Color green );
            ColorUtility.TryParseHtmlString(  "#f0d544", out Color yellow );
            _greenColor = green;
            _yellowColor = yellow;
        }

        public void PlayAnimation(float chance)
        {
            if (chance == 0) return;
            if (chance > 0)
            {
                Text.text = $"- {Math.Abs(chance)}%";
                Text.color = _yellowColor;
            }
            else
            {
                Text.text = $"+ {Math.Abs(chance)}%";
                Text.color = _greenColor;
            }

            Sequence sequence = DOTween.Sequence();
            Text.gameObject.SetActive(true);
            Text.transform.position = Start.position;
            Text.alpha = 0.1f;
            sequence.Join(Text.transform.DOMove(End.position, Duration));
            sequence.Join(Text.DOFade(1, Duration));
            sequence.OnComplete(Disable);
            sequence.Play();
        }

        public TextMeshProUGUI Text;
        public Transform Start;
        public Transform End;
        private Color _greenColor;
        private Color _yellowColor;

        private void Disable()
        {
            Text.gameObject.SetActive(false);
        }

        private const float Duration = 0.8f;
    }
}