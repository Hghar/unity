using Infrastructure.CompositeDirector.Executors;

namespace Model.Composites.Representation
{
    public interface IRepresentation : IProcessExecutor
    {
        void Represent();
    }
}