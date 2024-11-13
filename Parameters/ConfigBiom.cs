using System;
using System.Collections.Generic;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class ConfigBiom : IInfo<ConfigBiom>
    {
        [SerializeField] public List<Room> _rooms = new List<Room>();
        [SerializeField] public string Uid;
        [SerializeField] public string BiomName;
        [SerializeField] public string PrefabName;
        [SerializeField] public int StageNumber;
        [SerializeField] public int Coin_Quantity_Modifier;
        [SerializeField] public string FractionEnemyTags;

        public void Perform(ConfigBiom characterStore)
        {
            characterStore.Uid = Uid;
            characterStore.BiomName = BiomName;
            characterStore.FractionEnemyTags = FractionEnemyTags;
            characterStore._rooms = _rooms;
            characterStore.PrefabName = PrefabName;
            characterStore.StageNumber = StageNumber;
            characterStore.Coin_Quantity_Modifier = Coin_Quantity_Modifier;
        }
    }

    [Serializable]
    public struct Room
    {
        [SerializeField] public int Lvl_1;
        [SerializeField] public int Lvl_2;
        [SerializeField] public int Lvl_3;
        [SerializeField] public int Lvl_4;
        [SerializeField] public int Lvl_5;
        [SerializeField] public int Might;
        [SerializeField] public int Coins;

        public int CommonEnemyCount => Lvl_1 + Lvl_2 + Lvl_3 + Lvl_4;
    }
}