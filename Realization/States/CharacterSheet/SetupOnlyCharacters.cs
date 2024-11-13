using System.Collections.Generic;
using Plugins.Ship;
using Plugins.Ship.Sheets;
using Realization.States.EmptySheetHelper;

namespace Realization.States.CharacterSheet
{
    public class SetupOnlyCharacters : ISetup
    {
        public string StateLink { get; }
        public Dictionary<string, ISheetLoader> Sheets { get; }

        public SetupOnlyCharacters(string stateLink, ISheetLoader characters)
        {
            StateLink = stateLink;
            Sheets = new()
            {
                {"tutorial", new EmptyLoader()},
                {"characters", characters}
            };
        }
    }
}