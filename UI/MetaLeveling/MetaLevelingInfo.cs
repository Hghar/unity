using Model.Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class MetaLevelingInfo
    {
        public Class _class;

        public bool IsBayReset;

        public bool IsCannotBeReset;

        public string HeadingText;

        public int[] levels;

        public Cost[] GoldCost;

        public Cost[] TokkenCost;

        public int CrystalCost;

        public InformationText[] GeneralInformation;

        public InformationText[] ClassInformation;
    }

    public class InformationText
    {
        public InformationText(string name)
        {
            NameText = name;
        }

        public string NameText;
        public string WhiteText;
        public string GreenText;

        public bool isUpdate;
    }

    public class Cost
    {
        public Cost(Currency currency, int value , bool isBay)
        {
            Currency = currency;
            IsBay = isBay;

            Value = value;
        }

        public Currency Currency;

        public bool IsBay;

        public int Value;
    }
}
