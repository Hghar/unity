using CompositeDirectorWithGeneratingComposites.CompositeDirector;
using CompositeDirectorWithGeneratingComposites.CompositeDirector.CompositeGeneration;
using Model.Commands.Creation;
using Units;

namespace Model.Commands
{
    public interface IAffectable : IPoolItem
    {
        Result FixedDamage(int value);
        Result FixedHeal(int value);
        Result Perform(IMinionCommand commandDamage);
        Result Unperform(IMinionCommand commandDamage);
        Result RecalculatePriorities();
        Result DecreaseFixedEnergy(int value);
        Result IncreaseFixedEnergy(int value);
        Result IncreasePercentEnergy(int value);
        Result DecreasePercentEnergy(int value, string energyTarget);
        Result Stun();
        Result Unstun();
        Result Silence();
        Result Unsilence();
        Result Immortality();
        Result Unimmortal();
        Result DamageReflection(int parameter);
        Result UndoDamageReflection(int parameter);
    }
}