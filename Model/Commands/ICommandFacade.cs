using Model.Commands.Types;
using Units;

namespace Model.Commands
{
    public interface ICommandFacade
    {
        public ICommand MakeCommand(string value, IMinion caster = null);
    }
}