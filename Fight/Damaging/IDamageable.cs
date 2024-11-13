using Fight.Targeting;

namespace Fight.Damaging
{
    public interface IDamageable : ITarget
    {
        public void TakeDamage(IDamage damage, out bool isKilling);
    }
}