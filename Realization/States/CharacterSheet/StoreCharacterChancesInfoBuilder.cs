using System;
using System.Collections.Generic;
using Infrastructure.Shared.Extensions;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;

namespace Realization.States.CharacterSheet
{
    public class StoreCharacterChancesInfoBuilder : InfoBuilder<StoreCharacterChancesConfig>
    {
        private int _grade;
        private float _probability_1;
        private float _probability_2;
        private float _probability_3;
        private float _probability_4;
        private float _probability_5;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => int.TryParse(s, out _grade)));
            queue.Enqueue((s => Parse.Float(s, out _probability_1)));
            queue.Enqueue((s => Parse.Float(s, out _probability_2)));
            queue.Enqueue((s => Parse.Float(s, out _probability_3)));
            queue.Enqueue((s => Parse.Float(s, out _probability_4)));
            queue.Enqueue((s => Parse.Float(s, out _probability_5)));
        }

        protected override IInfo<StoreCharacterChancesConfig> GetInternal()
            => new StoreCharacterChancesConfig()
            {
                Grade = _grade,
                Probability_1 = _probability_1,
                Probability_2 = _probability_2,
                Probability_3 = _probability_3,
                Probability_4 = _probability_4,
                Probability_5 = _probability_5,
            };
    }
}