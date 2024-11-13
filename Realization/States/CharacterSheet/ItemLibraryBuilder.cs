using System;
using System.Collections.Generic;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;

namespace Realization.States.CharacterSheet
{
    public class ItemLibraryBuilder : InfoBuilder<ItemLibraryConfig>
    {
        private string _uid;
        private int _unifiedValue;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => { _uid = s; }));
            queue.Enqueue((s => { int.TryParse(s,out _unifiedValue); }));
        }

        protected override IInfo<ItemLibraryConfig> GetInternal()
            => new ItemLibraryConfig()
            {
                Item_UID = _uid,
                Unified_Value = _unifiedValue,
            };
    }
}