using System;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.UI
{
    public class MenuButton : MonoBehaviour
    {
        public event Action ButtonPressed;
        [SerializeField] private Button _button;
        
        private void Awake()
        {
            _button.onClick.AddListener(OnButtonPressed);
        }

        private void OnButtonPressed()
        {
            ButtonPressed?.Invoke();
        }
    }
}