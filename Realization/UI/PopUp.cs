using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.UI
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private float _downStartOffset = 10;
        [SerializeField] private float _showTime = 5;
        [SerializeField] private TMP_Text _mainText;

        private Vector3 _defaultPosition;
        private const float ShowTime = 1;
        private Dictionary<Image, float> ImageAlphfas = new();
        private Dictionary<TMP_Text, float> TextAlphfas = new();

        public TMP_Text MainText
        {
            get => _mainText;
            set => _mainText = value;
        }

        public async Task Show()
        {
            try
            {
                _defaultPosition = transform.position;
                transform.position += Vector3.down * _downStartOffset;
                List<Image> images = GetComponentsInChildren<Image>().ToList();
                List<TMP_Text> texts = GetComponentsInChildren<TMP_Text>().ToList();

                foreach (Image image in images)
                {
                    Color color = image.color;
                    ImageAlphfas.Add(image, color.a);
                    color.a = 0;
                    image.color = color;
                }

                foreach (TMP_Text text in texts)
                {
                    Color color = text.color;
                    TextAlphfas.Add(text, color.a);
                    color.a = 0;
                    text.color = color;
                }

                foreach (Image image in images)
                {
                    image.DOFade(ImageAlphfas[image], ShowTime);
                }

                foreach (TMP_Text text in texts)
                {
                    text.DOFade(TextAlphfas[text], ShowTime);
                }

                await transform.DOMove(_defaultPosition, ShowTime).AsyncWaitForCompletion();
                await Task.Delay(TimeSpan.FromSeconds(_showTime));
                await FadeWithDelay();
                if (gameObject != null)
                    Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.Log($"Error in pop up showing: {e}");
            }
        }

        private async Task FadeWithDelay()
        {
            List<Image> images = GetComponentsInChildren<Image>().ToList();
            foreach (Image image in images)
            {
                image.DOFade(0, ShowTime);
            }

            List<TMP_Text> texts = GetComponentsInChildren<TMP_Text>().ToList();
            foreach (TMP_Text text in texts)
            {
                text.DOFade(0, ShowTime);
            }


            await Task.Delay(TimeSpan.FromSeconds(ShowTime));
        }
    }
}