using System;
using System.Collections.Generic;
using Infrastructure.Shared.Extensions;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;

namespace Realization.States.CharacterSheet
{
    public class PriorityConfigBuilder : InfoBuilder<PriorityConfig>
    {
        private int _gladiator;
        private int _templar;
        private int _ranger;
        private int _assassin;
        private int _spiritMaster;
        private int _sorcerer;
        private int _cleric;
        private int _chanter;
        private float _goalAggression;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => {}));
            queue.Enqueue((s => int.TryParse(s, out _gladiator)));
            queue.Enqueue((s => int.TryParse(s, out _templar)));
            queue.Enqueue((s => int.TryParse(s, out _ranger)));
            queue.Enqueue((s => int.TryParse(s, out _assassin)));
            queue.Enqueue((s => int.TryParse(s, out _spiritMaster)));
            queue.Enqueue((s => int.TryParse(s, out _sorcerer)));
            queue.Enqueue((s => int.TryParse(s, out _cleric)));
            queue.Enqueue((s => int.TryParse(s, out _chanter)));
            queue.Enqueue((s => Parse.Float(s, out _goalAggression)));
        }

        protected override IInfo<PriorityConfig> GetInternal()
            => new PriorityConfig()
            {
                Gladiator = _gladiator,
                Templar = _templar,
                Ranger = _ranger,
                Assassin = _assassin,
                SpiritMaster = _spiritMaster,
                Sorcerer = _sorcerer,
                Cleric = _cleric,
                Chanter = _chanter,
                GoalAggression = _goalAggression
            };
    }
}