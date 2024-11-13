using System;
using UnitSelling.Picking;
using Zenject;

namespace UnitSelling
{
    public class LimitedSeller : ISeller, IDisposable
    {
        private readonly ISeller _seller;
        private ISellingPossiblityFlag _sellingPossiblityFlag;

        public event Action<ISellable> SellableSelling;
        public event Action SellableSelled;

        public LimitedSeller(ISeller seller)
        {
            _seller = seller;
            _seller.SellableSelling += OnSelling;
            _seller.SellableSelled += OnSelled;
        }

        [Inject]
        private void Construct(ISellingPossiblityFlag sellingPossiblityFlag)
        {
            _sellingPossiblityFlag = sellingPossiblityFlag;
        }

        public bool TrySell(IUnitSellingConfig config, int level) // TODO: create reason class for try-methods instead of bool
        {
            if (_sellingPossiblityFlag.Value == false)
                return false;

            return _seller.TrySell(config, level);
        }

        public void Dispose()
        {
            _seller.SellableSelling -= OnSelling;
            _seller.SellableSelled -= OnSelled;
        }

        private void OnSelled()
        {
            SellableSelled?.Invoke();
        }

        private void OnSelling(ISellable sellable)
        {
            SellableSelling?.Invoke(sellable);
        }
    }
}