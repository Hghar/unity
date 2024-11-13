using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class CharacterStore : IInfo<CharacterStore>
    {
        [SerializeField] public int Grade;
        [SerializeField] public int Price;
        [SerializeField] public int Sell_1;
        [SerializeField] public int Sell_2;
        [SerializeField] public int Sell_3;
        [SerializeField] public int Sell_4;
        [SerializeField] public int Sell_5;
        
        public void Perform(CharacterStore characterStore)
        {
            characterStore.Grade = Grade;
            characterStore.Price = Price;
            characterStore.Sell_1 = Sell_1;
            characterStore.Sell_2 = Sell_2;
            characterStore.Sell_3 = Sell_3;
            characterStore.Sell_4 = Sell_4;
            characterStore.Sell_5 = Sell_5;
        }
    }
}