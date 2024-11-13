using CountablePublishers;
using UI.Binding;
using UnityEngine;

namespace UI
{
    public class LimitedValueView : MonoBehaviour
    {
        [SerializeField] private BindedIntSlider _slider;
        [SerializeField] private BindedIntText _currentValueText;
        [SerializeField] private BindedIntText _maxValueText;
        [SerializeField] private GameObject _valueObject;

        private ILimitedCountablePublisher<int> _publisher;

        private void OnValidate()
        {
            if (_valueObject != null)
            {
                if (_valueObject.TryGetComponent(out ILimitedCountablePublisher<int> countablePublisher))
                {
                    _publisher = countablePublisher;
                }
                else
                {
                    Debug.LogWarning($"In {nameof(LimitedValueView)} of {gameObject.name} " +
                                     $"the {nameof(_valueObject)} should have {nameof(ILimitedCountablePublisher<int>)}.\n" +
                                     $"The field of {nameof(_valueObject)} will become null.");
                    _valueObject = null;
                    _publisher = null;
                }
            }

            TryInitUiElements();
        }

        private void Awake()
        {
            if (_valueObject != null &&
                _valueObject.TryGetComponent(out ILimitedCountablePublisher<int> countablePublisher))
            {
                _publisher = countablePublisher;
            }
        }

        private void Start()
        {
            TryInitUiElements();
        }

        public void SetPublisher(ILimitedCountablePublisher<int> publisher)
        {
            _publisher = publisher;
            TryInitUiElements();
        }

        private bool TryInitUiElements()
        {
            if (_publisher == null)
                return false;

            if (_currentValueText != null)
                _currentValueText.SetPublisher(_publisher);

            if (_maxValueText != null)
                _maxValueText.SetPublisher(_publisher.MaxValuePublisher);

            if (_slider != null)
                _slider.SetPublisher(_publisher);

            return true;
        }
    }
}