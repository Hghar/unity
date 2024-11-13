using System;
using Plugins.Ship.Sheets.InfoSheet;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class CharacterSet : IInfo<CharacterSet>
    {
        [SerializeField] public MinionClass Class;
        [SerializeField] public int UnitCount;
        [SerializeField] public string Effect;
        [SerializeField] public string TextRus;
        [SerializeField] public string TextEng;

        public CharacterSet()
        {
            
        }

        public CharacterSet(CharacterSet characterSet, Skill skill) 
        {
            Class = characterSet.Class;
            UnitCount = characterSet.UnitCount;
            Effect = characterSet.Effect;
            TextRus = skill.TextRus;
            TextEng = skill.TextEng;
        }

        public void Perform(CharacterSet characterStore)
        {
            characterStore.Class = Class;
            characterStore.UnitCount = UnitCount;
            characterStore.Effect = Effect;
            characterStore.TextRus = TextRus;
            characterStore.TextEng = TextEng;
        }
    }
}