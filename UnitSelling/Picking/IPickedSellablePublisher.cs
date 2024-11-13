using System;

namespace UnitSelling.Picking
{
    public interface IPickedSellablePublisher
    {
        public event Action<ISellable> ElementPicked;
        public event Action<ISellable> ElementUnpicked;
    }
}