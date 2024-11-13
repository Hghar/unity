using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Button[] _additional;

        [SerializeField] private GameObject _tutorialFinishMessage;
        [SerializeField] private GameObject _wholeGameFinishMessage;

        public event Action ButtonPressed;

        private void Awake()
        {
            _button.onClick.AddListener(OnPressed);
        }

        public void Enable()
        {
            if (gameObject.activeInHierarchy)
                return;

            gameObject.SetActive(true);
            //_button.onClick.AddListener(OnPressed);
            foreach (Button button in _additional)
            {
                button.onClick.AddListener(OnPressed);
            }

            if (name != "Canvas_Win")
                return;

            if (PlayerPrefs.GetInt("level") == 1)
            {
                foreach (Button button in _additional)
                {
                    button.gameObject.SetActive(false);
                }
            }
            else _button.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ButtonPressed = null;
        }

        public async Task ShowOn(bool isShowFinish = false, bool isShowTuturialFinish = false)
        {
            if (gameObject.Equals(null) == false && gameObject.activeInHierarchy == false)
                Enable();
            await FadeAll(1);
        }

        public async Task ShowOff()
        {
            if (_tutorialFinishMessage != null)
                _tutorialFinishMessage.SetActive(false);

            if (_wholeGameFinishMessage != null)
                _wholeGameFinishMessage.SetActive(false);

            await FadeAll(0);

            if (gameObject != null)
                Disable();
        }

        private async Task FadeAll(float endValue)
        {
            Image[] images = GetComponentsInChildren<Image>();
            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>();
            List<Task> tasks = new();
            foreach (Image image in images)
            {
                tasks.Add(image.DOFade(endValue, 0.5f).AsyncWaitForCompletion());
            }

            foreach (TMP_Text text in texts)
            {
                tasks.Add(text.DOFade(endValue, 0.5f).AsyncWaitForCompletion());
            }

            await Task.WhenAny(tasks);
        }

        private void OnPressed()
        {
            ButtonPressed?.Invoke();
        }

        public void Disable()
        {
            if (gameObject.activeInHierarchy == false)
                return;

            gameObject.SetActive(false);
            _button.onClick.RemoveListener(OnPressed);
            foreach (Button button in _additional)
            {
                button.onClick.RemoveListener(OnPressed);
            }
        }
    }
}