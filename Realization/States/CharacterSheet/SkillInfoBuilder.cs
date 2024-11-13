using System;
using System.Collections.Generic;
using Plugins.Ship.Sheets.InfoSheet;

namespace Realization.States.CharacterSheet
{
    public class SkillInfoBuilder : InfoBuilder<Skill>
    {
        private string _uid;
        private string _skill;
        private string _textRus;
        private string _textEng;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => _uid = s));
            queue.Enqueue((s => _skill = s));
            queue.Enqueue((s => _textRus = s));
            queue.Enqueue((s => _textEng = s));
        }

        protected override IInfo<Skill> GetInternal()
            => new Skill()
            {
                Uid = _uid,
                SkillValue = _skill,
                TextRus = _textRus,
                TextEng = _textEng
            };
    }
}