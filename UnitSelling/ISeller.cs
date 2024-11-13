namespace UnitSelling
{
    public interface ISeller : IReadonlySeller
    {
        public bool TrySell(IUnitSellingConfig config, int level);
    }
}