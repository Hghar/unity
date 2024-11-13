using System;
using CountablePublishers;
using Parameters;

namespace Fight
{
    public interface IHealth : ILimitedCountablePublisher<int>, IUnitParam
    {
        public event Action BecameZero;

        public float FillPercent { get; }
        float StartValue { get; }
        bool Immortality { get; set; }
        void UpdateParam(int value);
        void IncreaseMax(int value);
        void DecreaseMax(int value);
        void Lock();
        void Unlock();
    }
}