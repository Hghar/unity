namespace Model.Commands.Types
{
    public interface ICommand
    {
        float Duration { get; }
        
        void Perform();
        void Undo();
    }
    
    public interface IUpdatableCommand : ICommand
    {
        bool Performed { get; }
        void UpdateDuration(float duration);
    }
}