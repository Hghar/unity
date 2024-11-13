using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sounds
{
    public class ButtonClickSoundPlayer : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons;
        [SerializeField] private AudioSource _audioSource;


        private void OnEnable()
        {
            foreach (Button button in _buttons)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
        }

        private void OnDisable()
        {
            foreach (Button button in _buttons)
            {
                button.onClick.RemoveListener(OnButtonClicked);
            }
        }

        public void AddButton(Button button)
        {
            if (_buttons.Contains(button) == false && button != null)
            {
                button.onClick.AddListener(OnButtonClicked);
                _buttons.Add(button);
            }
        }

        public void RemoveButton(Button button)
        {
            _buttons.Remove(button);

            if (button != null)
                button.onClick.RemoveListener(OnButtonClicked);
        }

        public void SetNewButtons(IEnumerable buttons)
        {
            while (_buttons.Count > 0)
                RemoveButton(_buttons[0]);

            foreach (Button button in buttons)
            {
                AddButton(button);
            }
        }

        private void OnButtonClicked()
        {
            _audioSource.Play();
        }
    }
}