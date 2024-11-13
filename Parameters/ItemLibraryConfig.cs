using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;
using UnityEngine.Serialization;

namespace Parameters
{
    [Serializable]
    public class ItemLibraryConfig : IInfo<ItemLibraryConfig>
    {
        [SerializeField] public string Item_UID;
        [SerializeField] public int Unified_Value;
        
        public void Perform(ItemLibraryConfig characterStore)
        {
            characterStore.Item_UID = Item_UID;
            characterStore.Unified_Value = Unified_Value;
        }
    }
}