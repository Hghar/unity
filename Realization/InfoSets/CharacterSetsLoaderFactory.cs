using System.Collections.Generic;
using Parameters;
using Plugins.Ship.Sheets;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Realization.InfoSets
{
    public class CharacterSetsLoaderFactory : InfoLoaderFactory<CharacterSet>
    {
        public CharacterSetsLoaderFactory(string link, string postfix, IInfoBuilder<CharacterSet> builder, 
            List<CharacterSet> players, int startSkipCount = 1, int maxLength = -1, bool invert = false) : 
            base(link, postfix, builder, players, startSkipCount, maxLength, invert)
        {
        }

        public override ISheetLoader Get()
        {
            InfoRowHandler<CharacterSet> rowHandler = new InfoRowHandler<CharacterSet>(_builder);
            InfoSheetFactory<CharacterSet> factory = new InfoSheetFactory<CharacterSet>(_players);
            return new CharacterSetsSheetLoader<IInfo<CharacterSet>>
                (rowHandler, factory, _link, _postfix, _startSkipCount, _maxLength, _invert, new Vector2(2, 4));
        }
    }
}