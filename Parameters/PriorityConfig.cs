using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class PriorityConfig : IInfo<PriorityConfig>
    {
        [SerializeField] public int Gladiator;
        [SerializeField] public int Templar;
        [SerializeField] public int Ranger;
        [SerializeField] public int Assassin;
        [SerializeField] public int SpiritMaster;
        [SerializeField] public int Sorcerer;
        [SerializeField] public int Cleric;
        [SerializeField] public int Chanter;
        [SerializeField] public float GoalAggression;
        
        public void Perform(PriorityConfig characterStore)
        {
            characterStore.Gladiator = Gladiator;
            characterStore.Templar = Templar;
            characterStore.Ranger = Ranger;
            characterStore.Assassin = Assassin;
            characterStore.SpiritMaster = SpiritMaster;
            characterStore.Sorcerer = Sorcerer;
            characterStore.Cleric = Cleric;
            characterStore.Chanter = Chanter;
            characterStore.GoalAggression = GoalAggression;
        }
    }
}