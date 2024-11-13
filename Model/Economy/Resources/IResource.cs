using System;
using Model.Economy.Numerics;

namespace Model.Economy.Resources
{
    public interface IResource : INaturalNumber, IDisposable
    {
        public Currency Currency { get; }
        void Set(int value);
    }
}