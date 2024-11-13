using CountablePublishers;

namespace Fight
{
    public interface ICooldown : ICountablePublisher<float>
    {
        float StartValue { get; }
    }
}