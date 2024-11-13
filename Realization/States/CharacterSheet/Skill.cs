using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [Serializable]
    public class Skill : IInfo<Skill>
    {
        [SerializeField] private string _uid;
        [SerializeField] private string _skillValue;
        [SerializeField] private string _textRus;
        [SerializeField] private string _textEng;

        public string Uid
        {
            get => _uid;
            set => _uid = value;
        }

        public string SkillValue
        {
            get => _skillValue;
            set => _skillValue = value;
        }

        public string TextRus
        {
            get => _textRus;
            set => _textRus = value;
        }

        public string TextEng
        {
            get => _textEng;
            set => _textEng = value;
        }

        public void Perform(Skill info)
        {
            info._uid = _uid;
            info._skillValue = _skillValue;
            info._textRus = _textRus;
            info._textEng = _textEng;
        }
    }
}