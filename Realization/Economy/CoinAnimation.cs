using System;
using DG.Tweening;
using Model.Economy;
using TMPro;
using UnityEngine;
using Zenject;

namespace Realization.Economy
{
    public class CoinAnimation:MonoBehaviour
    {
        public TextMeshProUGUI Text;
        public Transform Start;
        public Transform End;
        private IStorage _storage;
        private Color _greenColor;
        private Color _redColor;

        [Inject]
        public void Construct(IStorage storage)
        {
            _storage = storage;
            _storage.PlayerProgress.WorldData.CurrencyData.GoldMinus += MinusAnimation;
            _storage.PlayerProgress.WorldData.CurrencyData.GoldPlus += PlusAnimation;
            ColorUtility.TryParseHtmlString("#a6e740", out Color green);
            ColorUtility.TryParseHtmlString("#fe4343", out Color red);
            _greenColor = green;
            _redColor = red;
        }

        private void OnDestroy()
        {
            _storage.PlayerProgress.WorldData.CurrencyData.GoldMinus -= MinusAnimation;
            _storage.PlayerProgress.WorldData.CurrencyData.GoldPlus -= PlusAnimation;
        }

        private void MinusAnimation(int count )
        {
            Text.text = $"-{count}";
            Text.color = _redColor;

            Sequence sequence = DOTween.Sequence();
            Text.gameObject.SetActive(true);
            Text.transform.position = Start.position;
            Text.alpha = 0.1f;
            sequence.Join(Text.transform.DOMove(End.position, Duration));
            sequence.Join(Text.DOFade(1, Duration));
            sequence.OnComplete(Disable);
            sequence.Play();
        }
        private void PlusAnimation(int count )
        {
            Text.text = $"+{count}";
            Text.color = _greenColor;

            Sequence sequence = DOTween.Sequence();
            Text.gameObject.SetActive(true);
            Text.transform.position = Start.position;
            Text.alpha = 0.1f;
            sequence.Join(Text.transform.DOMove(End.position, Duration));
            sequence.Join(Text.DOFade(1, Duration));
            sequence.OnComplete(Disable);
            sequence.Play();
        }

        private void Disable()
        {
            Text.gameObject.SetActive(false);
        }

        private const float Duration = 0.5f;
    }
}