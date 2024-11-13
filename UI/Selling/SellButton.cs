using Realization.General;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI.Selling
{
    public class SellButton : MonoBehaviour, ISellButton, ISwitchableSellButton, IInteractivitySwitchableSellButton
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _sellText;

        public event Action Clicked;
        public void Disable() => gameObject.SetActive(false);
        public void Enable() => gameObject.SetActive(true);

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            //_button.onClick.RemoveListener(OnButtonClicked);
        }

        public void SwitchOn()
        {
            _button.gameObject.SetActive(true);
        }

        public void SwitchOff()
        {
            if (_button == null)
                return;
            _button.gameObject.SetActive(false);
        }

        public void SwitchInteractivityOff()
        {
            _button.interactable = false;
        }

        public void SwitchInteractivityOn()
        {
            _button.interactable = true;
        }

        private void OnButtonClicked()
        {
            Clicked?.Invoke();
        }

        public void SetSellValue(float value)
        {
            _sellText.text = ((int)value).ToString();
        }
    }
}