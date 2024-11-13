using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Battle
{
    [RequireComponent(typeof(Button))]
    public class StartBattleButton : MonoBehaviour, IStartBattleButton
    {
        [SerializeField] private bool _dontChangeActive = false;
        
        private Button _button;

        [SerializeField] private Button _autoPlaceButton;
        [SerializeField] private Button _eatMinion;

        public event Action Clicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClicked);
        }

        public async void SetActive(bool value)
        {
            if (_dontChangeActive)
                return;

            if (value && gameObject.name == "Button_StartFight")
            {
                await System.Threading.Tasks.Task.Delay(600);
            }

            _autoPlaceButton.interactable = value;

            _eatMinion.interactable = value;

            gameObject.SetActive(value);

        }

        private void OnButtonClicked()
        {
            Clicked?.Invoke();
        }
    }
}