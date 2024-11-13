using System;
using Infrastructure.Services.StaticData;
using Model.Economy;
using Parameters;
using Units;
using UnityEngine;

namespace Infrastructure.Services.CharacteristicSetupService
{
    public class CharacteristicSetupService:ICharacteristicSetupService
    {
        private readonly IStaticDataService _staticDataService;
        private readonly IStorage _storage;

        public CharacteristicSetupService(IStaticDataService staticDataService,IStorage storage)
        {
            _staticDataService = staticDataService;
            _storage = storage;
        }
        public void SetupCharacteristic(IUnitParameters parameters,ClassParent id)
        {
            int level = 0;
            try
            {
                level = _storage.PlayerProgress.CoreUpgrades.Stats.GetLevel(id);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            if (NoBuff(level)) return;
            
            LevelingUpConfig config = _staticDataService.ForMinionStatsUpgrade(level);
            StatsData statsData = config.Stats.GetData(id);
            
            parameters.Health.IncreaseMax((int)statsData.Health);
            parameters.Damage.Increase(statsData.Power);
            parameters.Armor.Increase(statsData.Armor);
        }

        public void SetupGeneralCharacteristic(IUnitParameters parameters)
        {
            int level = 0;
            try
            {
                level = _storage.PlayerProgress.CoreUpgrades.CurrentGeneralLevel;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            if (NoBuff(level)) return;
            
            GeneralLevelingUpConfig config = _staticDataService.ForStatsUpgrade(level);

            parameters.Health.IncreaseMax(config.Health);
            parameters.Damage.Increase(config.Power);
            parameters.Armor.Increase(config.Armor);
            parameters.ChanceOfCriticalDamage.Increase(config.CriticalDamageChance);
            parameters.CriticalDamageMultiplier.Increase(config.CriticalDamageMultiplier);
            parameters.Agility.Increase(config.DodgeChance);
            parameters.Healing.UpdateParam((int)(parameters.Healing.Value + config.HealPower));
        }

        private bool NoBuff(int level)
        {
            return level == 0;
        }
    }
}