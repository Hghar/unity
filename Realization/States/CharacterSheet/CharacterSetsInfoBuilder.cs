using System;
using System.Collections.Generic;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;
using Units;

namespace Realization.States.CharacterSheet
{
    public class CharacterSetsInfoBuilder : InfoBuilder<CharacterSet>
    {
        private MinionClass _class;
        private int _unitCount;
        private string _effect;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue(AddClass);
            queue.Enqueue((s => int.TryParse(s, out _unitCount)));
            queue.Enqueue((s => _effect = s));
        }
        
        private void AddClass(string value)
        {
            if (Enum.TryParse<MinionClass>(value, out var result))
            {
                _class = result;
            }
        }

        protected override IInfo<CharacterSet> GetInternal()
            => new CharacterSet()
            {
                Class = _class,
                UnitCount = _unitCount,
                Effect = _effect
            };
    }
}