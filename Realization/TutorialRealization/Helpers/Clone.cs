using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.TutorialRealization.Helpers
{
    public class Clone : MonoBehaviour
    {
        private GameObject _gameObject;
        private RectTransform _mainTransform;
        private RectTransform _transform;
        private List<Image> _images;
        private List<TMP_Text> _texts;

        public void Init(GameObject gameObject)
        {
            _gameObject = gameObject;
            _transform = GetComponent<RectTransform>();
            _mainTransform = _gameObject.GetComponent<RectTransform>();
            _images = GetComponentsInChildren<Image>().ToList();
            _texts = GetComponentsInChildren<TMP_Text>().ToList();

            gameObject.layer = LayerMask.NameToLayer("Clickable");
            foreach (var image in _images)
            {
                image.gameObject.layer = LayerMask.NameToLayer("Clickable");
            }
            foreach (var text in _texts)
            {
                text.gameObject.layer = LayerMask.NameToLayer("Clickable");
            }
        }

        private void Update()
        {
            //todo write unit test and probably remove "is"
            if (_gameObject == null || _gameObject is null)
            {
                Destroy(gameObject);
                return;
            }

            if (_gameObject.activeInHierarchy == false)
            {
                foreach (Image image in _images)
                {
                    image.enabled = false;
                }

                foreach (TMP_Text text in _texts)
                {
                    text.enabled = false;
                }

                return;
            }

            if (_transform == null)
            {
                gameObject.transform.position = _gameObject.transform.position;
                return;
            }

            foreach (Image image in _images)
            {
                image.enabled = true;
            }

            foreach (TMP_Text text in _texts)
            {
                text.enabled = true;
            }

            _transform.position = _mainTransform.position;
            _transform.localScale = _mainTransform.localScale;
            // _transform.sizeDelta = _mainTransform.sizeDelta;
        }
    }
}