using System;
using Infrastructure.CompositeDirector;
using Infrastructure.CompositeDirector.Executors;
using Infrastructure.Shared.Factories;

namespace Entities.Factories
{
    public abstract class EntityFactory<T, TArgs> : IFactory<T, TArgs>, IFactory<TArgs> where T : IProcessExecutor
    {
        private readonly CompositeDirector _director;

        public event Action Created;

        public EntityFactory(CompositeDirector director)
        {
            _director = director;
        }

        public T Create(TArgs args)
        {
            T executor = CreateInternal(args);
            AddToDirector(executor);
            Created?.Invoke();
            return executor;
        }

        void IFactory<TArgs>.Create(TArgs args)
        {
            T executor = CreateInternal(args);
            AddToDirector(executor);
            Created?.Invoke();
        }

        protected abstract T CreateInternal(TArgs args);

        private void AddToDirector(IProcessExecutor executor)
        {
            _director.TryAddExecutor(executor);
        }
    }
}