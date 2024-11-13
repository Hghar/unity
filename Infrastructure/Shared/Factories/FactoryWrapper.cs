namespace Infrastructure.Shared.Factories
{
    public class FactoryWrapper<TArgs> : IFactory
    {
        private readonly IFactory<TArgs> _factory;
        private readonly TArgs _args;

        public FactoryWrapper(IFactory<TArgs> factory, TArgs args)
        {
            _factory = factory;
            _args = args;
        }

        public void Create()
        {
            _factory.Create(_args);
        }
    }
}