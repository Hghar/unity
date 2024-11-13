using System;
using Plugins.Ship.Sheets.InfoSheet;
using UnityEngine;

namespace Realization.Configs
{
    [Serializable]
    public class Constants : IInfo<Constants>
    { 
        [field: SerializeField] public int TavernBasicAmountOfGold { get; set; }
        [field: SerializeField] public int TavernBasicAmountOfCharacters { get; set; }
        [field: SerializeField] public Vector2 BattleDamageSpread { get; set; }
        [field: SerializeField] public Vector2 BattleHealingSpread { get; set; }
        [field: SerializeField] public int GeneralMaxRange { get; set; }
        [field: SerializeField] public float GeneralClockFrequency { get; set; }
        [field: SerializeField] public Vector3 LvlUpBoosterGrade1 { get; set; }
        [field: SerializeField] public Vector3 LvlUpBoosterGrade2 { get; set; }
        [field: SerializeField] public Vector3 LvlUpBoosterGrade3 { get; set; }
        [field: SerializeField] public Vector3 LvlUpBoosterGrade4 { get; set; }
        [field: SerializeField] public Vector3 LvlUpBoosterGrade5 { get; set; }
        [field: SerializeField] public int DungeonTargetNumberOfCoins { get; set; }
        [field: SerializeField] public float DungeonCoinsSpread { get; set; }
        [field: SerializeField] public Vector2 DungeonCommonUvSpread { get; set; }
        [field: SerializeField] public Vector2 DungeonBossUvSpread { get; set; }
        [field: SerializeField] public Vector5 BattleExperiencePoolGrade1 { get; set; }
        [field: SerializeField] public Vector5 BattleExperiencePoolGrade2 { get; set; }
        [field: SerializeField] public Vector5 BattleExperiencePoolGrade3 { get; set; }
        [field: SerializeField] public Vector5 BattleExperiencePoolGrade4 { get; set; }
        [field: SerializeField] public Vector5 BattleExperiencePoolGrade5 { get; set; }
        [field: SerializeField] public Vector4 LvlUpExperienceBoundaries { get; set; }
        [field: SerializeField] public int DungeonCostOfStoreUpdate { get; set; }
        [field: SerializeField] public Vector4 DungeonStoreUpdateCost { get; set; }
        [field: SerializeField] public int GeneralUpgradeResetCost { get; set; }
        [field: SerializeField] public int BattleGoalUnavailabilityTickTimer { get; set; }

        public void Perform(Constants constants)
        {
            constants.TavernBasicAmountOfGold = TavernBasicAmountOfGold;
            constants.BattleDamageSpread = BattleDamageSpread;
            constants.BattleHealingSpread = BattleHealingSpread;
            constants.GeneralMaxRange = GeneralMaxRange;
            constants.GeneralClockFrequency = GeneralClockFrequency;
            constants.LvlUpBoosterGrade1 = LvlUpBoosterGrade1;
            constants.LvlUpBoosterGrade2 = LvlUpBoosterGrade2;
            constants.LvlUpBoosterGrade3 = LvlUpBoosterGrade3;
            constants.LvlUpBoosterGrade4 = LvlUpBoosterGrade4;
            constants.LvlUpBoosterGrade5 = LvlUpBoosterGrade5;
            constants.DungeonCostOfStoreUpdate = DungeonCostOfStoreUpdate;
            constants.DungeonStoreUpdateCost = DungeonStoreUpdateCost;
            constants.GeneralUpgradeResetCost = GeneralUpgradeResetCost;
            constants.GeneralUpgradeResetCost = GeneralUpgradeResetCost;
            constants.DungeonTargetNumberOfCoins = DungeonTargetNumberOfCoins;
            constants.DungeonCoinsSpread = DungeonCoinsSpread;
            constants.DungeonCommonUvSpread = DungeonCommonUvSpread;
            constants.DungeonBossUvSpread = DungeonBossUvSpread;
            constants.BattleExperiencePoolGrade1 = BattleExperiencePoolGrade1;
            constants.BattleExperiencePoolGrade2 = BattleExperiencePoolGrade2;
            constants.BattleExperiencePoolGrade3 = BattleExperiencePoolGrade3;
            constants.BattleExperiencePoolGrade4 = BattleExperiencePoolGrade4;
            constants.BattleExperiencePoolGrade5 = BattleExperiencePoolGrade5;
            constants.LvlUpExperienceBoundaries = LvlUpExperienceBoundaries;
            constants.DungeonCostOfStoreUpdate = DungeonCostOfStoreUpdate;
            constants.DungeonStoreUpdateCost = DungeonStoreUpdateCost;
            constants.GeneralUpgradeResetCost = GeneralUpgradeResetCost;
            constants.TavernBasicAmountOfCharacters = TavernBasicAmountOfCharacters;
            constants.BattleGoalUnavailabilityTickTimer = BattleGoalUnavailabilityTickTimer;
        }
    }

    [Serializable]
    public class Vector5
    {
        public float Level_1, Level_2, Level_3, Level_4, Level_5;

        public Vector5(float a, float b, float c, float d, float e)
        {
            this.Level_1 = a;
            this.Level_2 = b;
            this.Level_3 = c;
            this.Level_4 = d;
            this.Level_5 = e;
        }
    }
}