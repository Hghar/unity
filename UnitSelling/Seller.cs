using System;
using Model.Economy;
using UnitSelling.Picking;
using Zenject;

namespace UnitSelling
{
    public class Seller : IReadonlySeller, ISeller
    {
        private IStorage _storage;
        private IPickedSellable _pickedSellable;

        public event Action<ISellable> SellableSelling;
        public event Action SellableSelled;

        [Inject]
        private void Construct(IStorage storage, IPickedSellable pickedSellable)
        {
            _storage = storage;
            _pickedSellable = pickedSellable;
        }

        public bool TrySell(IUnitSellingConfig config, int level)
        {
            if (_pickedSellable.Sellable != null)
            {
                if (_storage.AddResource(config.UnitPriceCurrency, config.UnitPriceValue[level - 1]))
                {
                    SellableSelling?.Invoke(_pickedSellable.Sellable);
                    SellableSelled?.Invoke();
                    return true;
                }
            }

            return false;
        }
    }
}