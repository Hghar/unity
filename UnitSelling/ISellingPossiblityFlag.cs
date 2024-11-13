using System;

namespace UnitSelling
{
    public interface ISellingPossiblityFlag
    {
        public bool Value { get; }

        public event Action ValueChanged;
    }
}