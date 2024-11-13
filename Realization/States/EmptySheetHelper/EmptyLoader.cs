using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets;

namespace Realization.States.EmptySheetHelper
{
    public class EmptyLoader : ISheetLoader
    {
        public ISheet Load(string name)
        {
            return new EmptySheet();
        }

        public Task Save(string name, string postfix)
        {
            return Task.CompletedTask;
        }
    }
}