using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.WindowService;
using Parameters;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;

namespace Infrastructure.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string DungeonConfigPath = "DungeonConfig";
        private const string WindowsPath = "StaticData/Windows/WindowStaticData";
        private const string PopupPath = "StaticData/Popups/PopupsStaticConfig";
        private const string CharacterConfigPath = "Characters Config";
        private const string GeneralLevelingConfigPath = "GeneralLevelingUp";
        private const string ClassLevelingUpConfigPath = "ClassLevelingUpConfig";

        private CharacterConfig _characterConfig;

        private Dictionary<WindowId, WindowData> _windows;
        private Dictionary<PopUpId, PopUpWindowData> _popups;
        private Dictionary<int, BiomeData> _bioms;
        private Dictionary<int, GeneralLevelingUpConfig> _generalStats;
        private Dictionary<int, LevelingUpConfig> _levelingUpConfigs;


        public void Load()
        {
            _characterConfig = Resources
                    .Load<CharacterConfig>(CharacterConfigPath);
            
            _windows = Resources
                    .Load<WindowStaticConfig>(WindowsPath)
                    .Configs
                    .ToDictionary(x => x.WindowId, x => x);

            _generalStats = Resources
                    .Load<GeneralLevelingConfig>(GeneralLevelingConfigPath)
                    .Configs
                    .ToDictionary(x => x.Level, x => x);

            _levelingUpConfigs = Resources
                    .Load<ClassLevelingUpConfig>(ClassLevelingUpConfigPath)
                    .Configs
                    .ToDictionary(x => x.Level, x => x);

            _bioms = Resources
                    .Load<DungeonConfig>(DungeonConfigPath)
                    .BiomeDatas
                    .ToDictionary(x => x.Key, x => x);

            _popups = Resources
                    .Load<PopupStaticConfig>(PopupPath)
                    .Configs
                    .ToDictionary(x => x.Id, x => x);
        }

		public PopUpWindowData ForPopUpWindow(PopUpId sittingId) =>
                _popups.TryGetValue(sittingId, out PopUpWindowData staticData)
                        ? staticData
                        : null;

        public int BiomsCount => _bioms.Count;

        public CharacterConfig CharacterConfig() => _characterConfig;

        public BiomeData ForBioms(int number) =>
                _bioms.TryGetValue(number, out BiomeData staticData)
                        ? staticData
                        : null;

        public WindowData ForWindow(WindowId id)
            => _windows.TryGetValue(id, out WindowData staticData)
                    ? staticData
                    : null;

        public GeneralLevelingUpConfig ForStatsUpgrade(int id) =>
                _generalStats.TryGetValue(id, out GeneralLevelingUpConfig config)
                        ? config
                        : null;

        public LevelingUpConfig ForMinionStatsUpgrade(int level) =>
                _levelingUpConfigs.TryGetValue(level, out LevelingUpConfig config)
                        ? config
                        : null;
    }
}