using Fight.Fractions;
using Helpers.Position;

namespace Fight.Targeting
{
    public interface ITarget : IDestroyablePoint
    {
        public Fraction Fraction { get; }
        public int Priority { get; }
        public bool IsDamaged { get; }
    }
}