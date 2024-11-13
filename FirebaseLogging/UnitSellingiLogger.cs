using Firebase.Analytics;
using UnitSelling;
using UnityEngine;
using Zenject;

namespace FirebaseLogging
{
    public class UnitSellingiLogger : MonoBehaviour
    {
        private IReadonlySeller _seller;

        [Inject]
        private void Construct(IReadonlySeller seller)
        {
            _seller = seller;
            _seller.SellableSelled += OnSellableSelled;
        }


        private void OnDestroy()
        {
            _seller.SellableSelled -= OnSellableSelled;
        }

        private void OnSellableSelled()
        {
            string message = LevelAttacher.AttachLevel(EventNames.MinionEaten);
            FirebaseAnalytics.LogEvent(message);
        }
    }
}