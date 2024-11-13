using System;
using System.Collections.Generic;
using Picking;

namespace UnitSelling.Picking
{
    public interface ISellablePool : IPickablePool<ISellable>
    {
        public IEnumerable<ISellable> Sellables { get; }

        public event Action SellableAdded;
        public event Action SellableRemoved;
    }
}