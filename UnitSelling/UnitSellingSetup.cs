using Model.Economy;

namespace UnitSelling
{
    public class UnitSellingSetup : IUnitSellingConfig
    {
        private CurrencyValuePair _unitPrice;

        public UnitSellingSetup(CurrencyValuePair currencyValuePair)
        {
            _unitPrice = currencyValuePair;
        }

        public Currency UnitPriceCurrency => _unitPrice.Currency;
        public int[] UnitPriceValue => _unitPrice.Value;
    }
}