using Model.Commands.Types;

namespace Model.Commands.Actions
{
    public class EmptyCommand : ICommand
    {
        public float Duration => 0;
        
        public void Perform()
        {
            
        }

        public void Undo()
        {
            
        }
    }
}