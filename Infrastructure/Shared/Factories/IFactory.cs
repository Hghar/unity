namespace Infrastructure.Shared.Factories
{
    public interface IFactory<TOutput, TArgs>
    {
        TOutput Create(TArgs args);
    }

    public interface IFactory<TArgs>
    {
        void Create(TArgs args);
    }

    public interface IFactory
    {
        void Create();
    }
}