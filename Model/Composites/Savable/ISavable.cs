using Infrastructure.CompositeDirector.Executors;

namespace Model.Composites.Savable
{
    public interface ISavable : IProcessExecutor
    {
        void Save();
    }
}