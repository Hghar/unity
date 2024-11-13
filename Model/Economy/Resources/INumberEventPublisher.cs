using Model.Economy.Numerics;

namespace Model.Economy.Resources
{
    public interface INumberEventPublisher
    {
        event ValueChanging ValueChanged;
    }
}