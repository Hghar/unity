using System.Collections.Generic;
using Infrastructure.Services.StaticData;
using Parameters;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [CreateAssetMenu(fileName = "New Dungeon Config", menuName = "Configs/Dungeon", order = 0)]
    public class DungeonConfig : ScriptableObject
    {
        public List<BiomeData> BiomeDatas;
        public List<LootingConfig> Looting;
    
        public void UpdateLooting(List<LootingConfig> lootingConfigs)
        {
            Looting = new List<LootingConfig>(lootingConfigs);
        }
    }
}