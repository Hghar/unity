using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class LootingConfig : IInfo<LootingConfig>
    {
        [SerializeField] public string Item_UID;
        [SerializeField] public float Common_chance;
        [SerializeField] public float Boss_chance;
        
        public void Perform(LootingConfig characterStore)
        {
            characterStore.Item_UID = Item_UID;
            characterStore.Common_chance = Common_chance;
            characterStore.Boss_chance = Boss_chance;
        }
    }
}