using Fight.Fractions;

namespace Fight.Healing
{
    public interface IHealable
    {
        public Fraction Fraction { get; }

        public void Heal(int healingValue);
    }
}