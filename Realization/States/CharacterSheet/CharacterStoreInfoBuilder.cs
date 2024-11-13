using System;
using System.Collections.Generic;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;

namespace Realization.States.CharacterSheet
{
    public class CharacterStoreInfoBuilder : InfoBuilder<CharacterStore>
    {
        private int _grade;
        private int _price;
        private int _sell_1;
        private int _sell_2;
        private int _sell_3;
        private int _sell_4;
        private int _sell_5;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => int.TryParse(s, out _grade)));
            queue.Enqueue((s => int.TryParse(s, out _price)));
            queue.Enqueue((s => int.TryParse(s, out _sell_1)));
            queue.Enqueue((s => int.TryParse(s, out _sell_2)));
            queue.Enqueue((s => int.TryParse(s, out _sell_3)));
            queue.Enqueue((s => int.TryParse(s, out _sell_4)));
            queue.Enqueue((s => int.TryParse(s, out _sell_5)));
        }

        protected override IInfo<CharacterStore> GetInternal()
            => new CharacterStore()
            {
                Grade = _grade,
                Price = _price,
                Sell_1 = _sell_1,
                Sell_2 = _sell_2,
                Sell_3 = _sell_3,
                Sell_4 = _sell_4,
                Sell_5 = _sell_5,
            };
    }
}