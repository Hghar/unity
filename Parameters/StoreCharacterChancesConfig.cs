using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Parameters
{
    [Serializable]
    public class StoreCharacterChancesConfig : IInfo<StoreCharacterChancesConfig>
    {
        [SerializeField] public int Grade;
        [SerializeField] public float Probability_1;
        [SerializeField] public float Probability_2;
        [SerializeField] public float Probability_3;
        [SerializeField] public float Probability_4;
        [SerializeField] public float Probability_5;
        
        public void Perform(StoreCharacterChancesConfig characterStore)
        {
            characterStore.Grade = Grade;
            characterStore.Probability_1 = Probability_1;
            characterStore.Probability_2 = Probability_2;
            characterStore.Probability_3 = Probability_3;
            characterStore.Probability_4 = Probability_4;
            characterStore.Probability_5 = Probability_5;
        }
    }
}