using System;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Shared.Extensions;
using Plugins.Ship.Sheets.InfoSheet;
using Realization.Configs;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    public class ConstantsInfoBuilder : InfoBuilder<Constants>
    {
        private int _tavernBasicAmountOfGold;
        private int _tavernBasicAmountOfCharacters;
        private Vector2 _battleDamageSpread;
        private Vector2 _battleHealingSpread;
        private int _generalMaxRange;
        private float _generalClockFrequency;
        private Vector3 _lvlUpBoosterGrade1;
        private Vector3 _lvlUpBoosterGrade2;
        private Vector3 _lvlUpBoosterGrade3;
        private Vector3 _lvlUpBoosterGrade4;
        private Vector3 _lvlUpBoosterGrade5;
        private int _dungeonTargetNumberOfCoins;
        private float _dungeonCoinsSpread;
        private Vector2 _dungeonCommonUvSpread;
        private Vector2 _dungeonBossUvSpread;
        private Vector5 _battleExperiencePoolGrade1;
        private Vector5 _battleExperiencePoolGrade2;
        private Vector5 _battleExperiencePoolGrade3;
        private Vector5 _battleExperiencePoolGrade4;
        private Vector5 _battleExperiencePoolGrade5;
        private Vector4 _lvlUpExperienceBoundaries;
        private int _dungeonCostOfStoreUpdate;
        private Vector4 _dungeonStoreUpdateCost;
        private int _generalUpgradeResetCost;
        private int _battleGoalUnavailabilityTickTimer;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => {}));
            queue.Enqueue((s => int.TryParse(s, out _tavernBasicAmountOfGold)));
            queue.Enqueue((s => _battleDamageSpread = ConvertToVector(s)));
            queue.Enqueue((s => _battleHealingSpread = ConvertToVector(s)));
            queue.Enqueue((s => int.TryParse(s, out _generalMaxRange)));
            queue.Enqueue((s => Parse.Float(s, out _generalClockFrequency)));

            queue.Enqueue((s => _lvlUpBoosterGrade1 = ConvertToVector3(s)));
            queue.Enqueue((s => _lvlUpBoosterGrade2 = ConvertToVector3(s)));
            queue.Enqueue((s => _lvlUpBoosterGrade3 = ConvertToVector3(s)));
            queue.Enqueue((s => _lvlUpBoosterGrade4 = ConvertToVector3(s)));
            queue.Enqueue((s => _lvlUpBoosterGrade5 = ConvertToVector3(s)));
            
            queue.Enqueue((s => int.TryParse(s, out _dungeonTargetNumberOfCoins)));
            queue.Enqueue((s => Parse.Float(s, out _dungeonCoinsSpread)));
            queue.Enqueue((s => _dungeonCommonUvSpread = ConvertToVector(s)));
            queue.Enqueue((s => _dungeonBossUvSpread = ConvertToVector(s)));

            queue.Enqueue((s => _battleExperiencePoolGrade1 = ConvertToVector5(s)));
            queue.Enqueue((s => _battleExperiencePoolGrade2 = ConvertToVector5(s)));
            queue.Enqueue((s => _battleExperiencePoolGrade3 = ConvertToVector5(s)));
            queue.Enqueue((s => _battleExperiencePoolGrade4 = ConvertToVector5(s)));
            queue.Enqueue((s => _battleExperiencePoolGrade5 = ConvertToVector5(s)));
            
            queue.Enqueue((s => _lvlUpExperienceBoundaries = ConvertToVector4(s)));
            queue.Enqueue((s => int.TryParse(s, out _dungeonCostOfStoreUpdate)));
            queue.Enqueue((s => _dungeonStoreUpdateCost = ConvertToVector4(s)));
            queue.Enqueue((s => int.TryParse(s, out _generalUpgradeResetCost)));
            queue.Enqueue((s => int.TryParse(s, out _tavernBasicAmountOfCharacters)));
            
            queue.Enqueue((s => {}));//Summon_BaseSummon_RarityChances
            queue.Enqueue((s => {}));//Summon_AdvancedSummon_RarityChances
            queue.Enqueue((s => {}));//Summon_AdvancedSummon_SingleCost
            queue.Enqueue((s => {}));//Summon_AdvancedSummon_MassCost
            queue.Enqueue((s => int.TryParse(s, out _battleGoalUnavailabilityTickTimer)));
        }

        private Vector2 ConvertToVector(string value)
        {
            var separator = value.IndexOf(':');

            float x = float.Parse(value.Substring(0, separator).ToString(), CultureInfo.InvariantCulture);
            float y = float.Parse(value.Substring(separator + 1, value.Length - separator - 1).ToString(),
                CultureInfo.InvariantCulture);
            return new Vector2(x, y);
        }
        private Vector3 ConvertToVector3(string value)
        {
            var parts = value.Split(':');
            float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[2], CultureInfo.InvariantCulture);
            return new Vector3(x, y, z);
        }
        
        private Vector4 ConvertToVector4(string value)
        {
            var parts = value.Split(':');
            float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float w = float.Parse(parts[3], CultureInfo.InvariantCulture);
            return new Vector4(x, y, z, w);
        }

        private Vector5 ConvertToVector5(string value)
        {
            var parts = value.Split(':');
            float x = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float z = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float w = float.Parse(parts[3], CultureInfo.InvariantCulture);
            float e = float.Parse(parts[4], CultureInfo.InvariantCulture);
            return new Vector5(x, y, z, w, e);
        }

        protected override IInfo<Constants> GetInternal()
            => new Constants()
            {
                TavernBasicAmountOfGold = _tavernBasicAmountOfGold,
                BattleDamageSpread = _battleDamageSpread,
                BattleHealingSpread = _battleHealingSpread,
                GeneralMaxRange = _generalMaxRange,
                GeneralClockFrequency = _generalClockFrequency,
                LvlUpBoosterGrade1 = _lvlUpBoosterGrade1,
                LvlUpBoosterGrade2 = _lvlUpBoosterGrade2,
                LvlUpBoosterGrade3 = _lvlUpBoosterGrade3,
                LvlUpBoosterGrade4 = _lvlUpBoosterGrade4,
                LvlUpBoosterGrade5 = _lvlUpBoosterGrade5,
                DungeonTargetNumberOfCoins = _dungeonTargetNumberOfCoins,
                DungeonCoinsSpread = _dungeonCoinsSpread,
                DungeonCommonUvSpread = _dungeonCommonUvSpread,
                DungeonBossUvSpread = _dungeonBossUvSpread,
                BattleExperiencePoolGrade1 = _battleExperiencePoolGrade1,
                BattleExperiencePoolGrade2 = _battleExperiencePoolGrade2,
                BattleExperiencePoolGrade3 = _battleExperiencePoolGrade3,
                BattleExperiencePoolGrade4 = _battleExperiencePoolGrade4,
                BattleExperiencePoolGrade5 = _battleExperiencePoolGrade5,
                LvlUpExperienceBoundaries = _lvlUpExperienceBoundaries,
                DungeonCostOfStoreUpdate = _dungeonCostOfStoreUpdate,
                DungeonStoreUpdateCost = _dungeonStoreUpdateCost,
                GeneralUpgradeResetCost = _generalUpgradeResetCost,
                TavernBasicAmountOfCharacters = _tavernBasicAmountOfCharacters,
                BattleGoalUnavailabilityTickTimer = _battleGoalUnavailabilityTickTimer
            };
    }
}