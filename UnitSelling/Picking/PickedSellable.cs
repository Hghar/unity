using Picking;

namespace UnitSelling.Picking
{
    public class PickedSellable : Picker<ISellablePool, ISellable>, IPickedSellable, IPickedSellablePublisher
    {
        public ISellable Sellable
        {
            get
            {
                TryGetPicked(out ISellable picked);
                return picked;
            }
        }
    }
}