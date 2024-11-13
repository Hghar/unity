using System;
using Units;
using UnitSelling.Picking;
using UnityEngine;
using Zenject;

namespace UnitSelling.Behaviours
{
    public class SellingHandler : MonoBehaviour
    {
        [SerializeField] private SellableBehaviour _sellableBehaviour;
        [SerializeField] private GameObject _rootParent;

        private IReadonlySeller _seller;
        public event Action<IMinion> Sell;

        [Inject]
        private void Construct(IReadonlySeller seller)
        {
            _seller = seller;
        }

        private void OnEnable()
        {
            _seller.SellableSelling += OnSelling;
        }

        private void OnDisable()
        {
            _seller.SellableSelling -= OnSelling;
        }

        private void OnSelling(ISellable sellable)
        {
            if (sellable == _sellableBehaviour.Sellable)
                HandleSelling();
        }

        private void HandleSelling()
        {
            Sell?.Invoke(_rootParent.GetComponent<IMinion>());
            Destroy(_rootParent);
        }
    }
}