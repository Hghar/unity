using System;
using System.Collections.Generic;
using Infrastructure.Shared.Extensions;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;

namespace Realization.States.CharacterSheet
{
    public class LootingBuilder : InfoBuilder<LootingConfig>
    {
        private string _uid;
        private float _commonChance;
        private float _bossChance;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => { _uid = s; }));
            queue.Enqueue((s => { Parse.Float(s,out _commonChance); }));
            queue.Enqueue((s => { Parse.Float(s,out _bossChance); }));
        }

        protected override IInfo<LootingConfig> GetInternal()
            => new LootingConfig()
            {
                    Item_UID = _uid,
                    Common_chance = _commonChance,
                    Boss_chance = _bossChance,
            };
    }
}