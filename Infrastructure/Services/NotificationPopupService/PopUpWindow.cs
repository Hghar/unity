using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Infrastructure.Services.NotificationPopupService
{
    public class PopUpWindow : MonoBehaviour, IPopup
    {
        private const float Duration = 0.55f;
        [SerializeField] private Button _close;
        [SerializeField] private CanvasGroup _group;
        [SerializeField] private TMP_Text _tmpText;
        
        private Sequence _showSequence;

        public void Initialize(Vector3 position, bool withButton, string text)
        {
            _close.gameObject.SetActive(withButton);
            transform.position = position;
            _tmpText.text = text;
        }

        private void Awake()
        {
            _group.alpha = 0;
            transform.localScale = Vector3.zero;
            _showSequence = DOTween.Sequence();
            _showSequence.Join(transform.DOScale(Vector3.one, Duration));
            _showSequence.Join(_group.DOFade(1, Duration));
            _showSequence.SetEase(Ease.OutCubic);
            _showSequence.Play();
            SubscribeButtons();
        }

        private void SubscribeButtons() => 
                _close.onClick.AddListener(DestroyPopup);

        public void DestroyPopup()
        {
            _showSequence?.Kill();
            _close.onClick.RemoveListener(DestroyPopup);
            Sequence sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(Vector3.zero, Duration));
            sequence.Join(_group.DOFade(0, Duration));
            sequence.SetEase(Ease.OutCubic);
            sequence.OnComplete(DestroyWindow);
            sequence.Play();
            
        }

        private void DestroyWindow()
        {
            Destroy(gameObject);
        }
    }
}