using System;
using System.Collections.Generic;
using System.Linq;
using Plugins.Ship.Sheets.InfoSheet;
using Units;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class GeneralLevelingUpConfig : IInfo<GeneralLevelingUpConfig>
    {
        [SerializeField] public int Level;
        [SerializeField] public int GoldCost;

        [SerializeField] public int Health;
        [SerializeField] public int Armor;
        [SerializeField] public float Power;
        [SerializeField] public float CriticalDamageChance;
        [SerializeField] public float CriticalDamageMultiplier;
        [SerializeField] public float DodgeChance;
        [SerializeField] public float HealPower;


        public void Perform(GeneralLevelingUpConfig characterStore)
        {
            characterStore.Level = Level;
            characterStore.GoldCost = GoldCost;
            characterStore.Health = Health;
            characterStore.Power = Power;
            characterStore.Armor = Armor;
            characterStore.CriticalDamageChance = CriticalDamageChance;
            characterStore.CriticalDamageMultiplier = CriticalDamageMultiplier;
            characterStore.DodgeChance = DodgeChance;
            characterStore.HealPower = HealPower;
        }
    }
    [Serializable]
    public class LevelingUpConfig : IInfo<LevelingUpConfig>
    {
        [SerializeField] public int Level;
        [SerializeField] public float GoldCost;
        [SerializeField] public float TokenCost;
        [SerializeField] public StatsConfig Stats;

        public void Perform(LevelingUpConfig characterStore)
        {
            characterStore.Level = Level;
            characterStore.GoldCost = GoldCost;
            characterStore.TokenCost = TokenCost;
            characterStore.Stats = Stats;
        }
    }

    [Serializable]
    public class StatsConfig
    {
        public List<StatsData> List;

        public StatsData GetData(ClassParent id) => 
                List.First(x=>x.ID == id);

        public StatsConfig()
        {
            List = new List<StatsData>();
        }
    }

    [Serializable]
    public class StatsData
    {
        public  ClassParent ID;
        public float Health ;
        public float Armor ;
        public float Power ;

        public StatsData(ClassParent id)
        {
            ID = id;
        }
    }
}