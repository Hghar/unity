using Infrastructure.CompositeDirector.Executors;

namespace Model.Composites.Hidable
{
    public interface IHidable : IProcessExecutor
    {
        void Hide();
    }
}