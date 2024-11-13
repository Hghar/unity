using CountablePublishers;
using TMPro;
using UnityEngine;

namespace UI.Binding
{
    public class BindedText<T> : ValueSubscriber<ICountablePublisher<T>, T>
    {
        [SerializeField] TMP_Text _text;

        private void OnValidate()
        {
            _text ??= GetComponent<TMP_Text>();
        }

        protected override void OnValueDecreased(T delta)
        {
            UpdateText();
        }

        protected override void OnValueIncreased(T delta)
        {
            UpdateText();
        }

        protected override void OnPublisherSetting(ICountablePublisher<T> value)
        {
            UpdateText();
        }

        protected virtual string ConvertValueToString(T value)
        {
            return value.ToString();
        }

        private void UpdateText()
        {
            _text.text = ConvertValueToString(Publisher.Value);
        }
    }
}