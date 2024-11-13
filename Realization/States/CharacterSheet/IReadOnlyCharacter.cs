using Units;

namespace Realization.States.CharacterSheet
{
    public interface IReadOnlyCharacter
    {
        string Uid { get; }
        MinionClass Class { get; }
        string Prefab { get; }
        int Grade { get; }
        string Tags { get; }
        int Health { get; }
        float Armor { get; }
        float Power { get; }
        float TimeBetweenAttacks { get; }
        int Range { get; }
        float CriticalDamageChance { get; }
        float CriticalDamageMultiplier { get; }
        float ChanceOfDodge { get; }
        int Energy { get; }
        int PowerOfHealing { get; }
        float AdditionalParameter { get; }

        int Level { get; }
    }
}