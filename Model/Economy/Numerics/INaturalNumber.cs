using System;
using Model.Economy.Resources;

namespace Model.Economy.Numerics
{
    public interface INaturalNumber : INumberEventPublisher
    {
        public int Value { get; }

        public void Add(int addableValue);
        public bool CanSubtract(int subtractValue);
        public bool TrySubtract(int subtractValue);
    }

    public delegate void ValueChanging(int different);
}