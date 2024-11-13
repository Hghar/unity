using CountablePublishers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Binding
{
    public class BindedIntSlider : ValueSubscriber<ILimitedCountablePublisher<int>, int>
    {
        [SerializeField] private Slider _slider;

        private ILimitedCountablePublisher<int> _value;

        private void OnDestroy()
        {
            UnsubscribeFromMaxValue();
        }

        protected override void OnValueDecreased(int delta)
        {
            ActualiseSliderValue();
        }

        protected override void OnValueIncreased(int delta)
        {
            ActualiseSliderValue();
        }

        protected override void OnPublisherSetting(ILimitedCountablePublisher<int> limitedCountablePublisher)
        {
            _value = limitedCountablePublisher;
            ActualiseSliderValue();
            UnsubscribeFromMaxValue();
            _value.MaxValueIncreasedBy += OnMaxValueIncreasedBy;
            _value.MaxValueDecreasedBy += OnMaxValueDecreasedBy;
        }

        protected override void OnDestroying()
        {
            UnsubscribeFromMaxValue();
        }

        private void OnMaxValueIncreasedBy(int delta)
        {
            ActualiseSliderValue();
        }

        private void OnMaxValueDecreasedBy(int delta)
        {
            ActualiseSliderValue();
        }

        private void ActualiseSliderValue()
        {
            _slider.value = (_slider.maxValue * Publisher.Value) / Publisher.MaxValue;
        }

        private void UnsubscribeFromMaxValue()
        {
            if (_value != null)
            {
                _value.MaxValueIncreasedBy -= OnMaxValueIncreasedBy;
                _value.MaxValueDecreasedBy -= OnMaxValueDecreasedBy;
            }
        }
    }
}