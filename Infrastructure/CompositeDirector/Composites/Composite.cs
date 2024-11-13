using Infrastructure.CompositeDirector.CompositeProviders;
using Infrastructure.CompositeDirector.Executors;

namespace Infrastructure.CompositeDirector.Composites
{
    public class Composite<T> where T : IProcessExecutor
    {
        private readonly CompositeDirector _director;

        public Composite(CompositeDirector director)
        {
            _director = director;
        }

        public ICompositeContainer<T> Select()
            => _director.SelectComposite<T>();
    }
}