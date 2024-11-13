using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.NotificationPopupService;
using Infrastructure.Services.WindowService;
using Parameters;
using Realization.States.CharacterSheet;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService
    {
        void Load();
        int BiomsCount { get; }
        BiomeData ForBioms(int number);
        CharacterConfig CharacterConfig();
        WindowData ForWindow(WindowId id);
        PopUpWindowData ForPopUpWindow(PopUpId sittingId);       
        GeneralLevelingUpConfig ForStatsUpgrade(int id);
        LevelingUpConfig ForMinionStatsUpgrade(int level);
        }

    [Serializable]
    public class BiomeData
    {
        public int Key;
        public string Name;
        public string PrefabName;
        public List<ConfigBiom> Configs = new List<ConfigBiom>();

        public ConfigBiom ForStage(int stage) => 
                Configs.First(x=>x.StageNumber == stage);
    }
}