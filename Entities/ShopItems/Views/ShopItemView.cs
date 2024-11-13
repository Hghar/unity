using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Entities.ShopItems.Views
{
    [RequireComponent(typeof(Button))]
    public class ShopItemView : MonoBehaviour, IDisposable
    {
        private const string ResourceIcon = " <sprite=1>";

        [SerializeField] private ShopCharacterParticl _particls;

        public int Class;
        public int Grade;
        public bool isHint = false;

        public Image Icon;
        public Image Frame;
        public TMP_Text Price;
        public Image ClassIcon;
        public Button _Button;

        public event Action<object> Clicked;

        private void OnEnable()
        {
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(OnClicked);
        }

        public void EffectNewSet(bool isActive)
        {
            if (Grade > 0 && isActive)
            {
                _particls.SetActiveHintEffect(true, Grade);
            }
            else
            {
                _particls.SetActiveHintEffect(false);
            }
        }

        public void EffectNewCharacter(bool isActive)
        {
            if(Class >= 0 && isActive)
            {
                _particls.SetActiveClassEffect(true, Class);
            }
            else
            {
                _particls.SetActiveClassEffect(false);
            }
        }

        private void OnDisable()
        {
            _Button.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked?.Invoke(this);
        }

        public void Dispose()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }
}