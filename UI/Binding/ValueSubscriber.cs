using CountablePublishers;
using UnityEngine;

namespace UI.Binding
{
    public abstract class ValueSubscriber<TPublisher, TType> : MonoBehaviour
        where TPublisher : ICountablePublisher<TType>
    {
        private TPublisher _publisher;

        protected TPublisher Publisher => _publisher;

        private void OnDestroy()
        {
            TryUnsubscribeFromPublisher();
            OnDestroying();
        }

        public void SetPublisher(TPublisher publisher)
        {
            TryUnsubscribeFromPublisher();
            _publisher = publisher;
            _publisher.IncreasedBy += OnValueIncreased;
            _publisher.DecreasedBy += OnValueDecreased;
            OnPublisherSetting(_publisher);
        }

        protected virtual void OnValueDecreased(TType delta)
        {
        }

        protected virtual void OnValueIncreased(TType delta)
        {
        }

        protected virtual void OnPublisherSetting(TPublisher publisher)
        {
        }

        protected virtual void OnDestroying()
        {
        }

        private bool TryUnsubscribeFromPublisher()
        {
            if (_publisher == null)
                return false;

            _publisher.IncreasedBy -= OnValueIncreased;
            _publisher.DecreasedBy -= OnValueDecreased;
            return true;
        }
    }
}