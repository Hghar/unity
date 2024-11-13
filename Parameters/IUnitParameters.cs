using Fight;
using Fight.Attack;
using Fight.Damaging;
using UnitSelling;

namespace Parameters
{
    public interface IUnitParameters
    {
        IHealth Health { get; }
        IDamage Damage { get; }
        IArmor Armor { get; }
        ICooldown Cooldown { get; }
        Agility Agility { get; } //DodgeChance
        Healing Healing { get; } // HealPower
        AttackRadius AttackRadius { get; }
        CriticalChance ChanceOfCriticalDamage { get; }
        CriticalMultiplier CriticalDamageMultiplier { get; }
        IUnitSellingConfig CellConfig { get; }
        public Energy Energy { get; }

        public Level Level { get; }
        bool TryApplyModificator(IParamModificator paramModificator);
    }
}