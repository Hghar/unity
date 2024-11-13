using CountablePublishers;

namespace Fight.Damaging
{
    public interface IDamage : ICountablePublisher<float>
    {
        float StartValue { get; }

        public void UpdateParam(int value);
    }
}