using Units;

namespace Realization.VisualEffects
{
    public interface IVisualEffectService
    {
        void Create(VisualEffectType type, IMinion target);
        void CreateAOE(VisualEffectType type, IMinion target, int radius);
        void CreateEffectWithDuration(VisualEffectType type, IMinion target);
        void EndEffectWithDuration(VisualEffectType type, IMinion target);
        void EndEffectWithDurationDirty(VisualEffectType type, IMinion target);
    }

    public enum VisualEffectType
    {
        Damage,
        SplashDamage,
        CriticalDamage,
        Heal,
        Healing,
        SplashHeal,
        Lifesteal,
        SplashLifesteal,
        Stun,
        Stunning,
        Silence,
        Silencing,
        Shield,
        Immortality,
        Reflection,
        Buff,
        Summon,
        Death,
        Evade,
        Energy,
        Debuff,
        AggressionUp,
        AggressionDown,
        EnergyDown
    }
}