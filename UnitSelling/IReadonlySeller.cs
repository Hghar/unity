using System;
using UnitSelling.Picking;

namespace UnitSelling
{
    public interface IReadonlySeller
    {
        public event Action SellableSelled;
        public event Action<ISellable> SellableSelling;
    }
}