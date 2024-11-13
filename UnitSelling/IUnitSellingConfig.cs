using Model.Economy;

namespace UnitSelling
{
    public interface IUnitSellingConfig
    {
        public Currency UnitPriceCurrency { get; }
        public int[] UnitPriceValue { get; }
    }
}