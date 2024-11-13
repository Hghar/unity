using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.StaticData;
using NaughtyAttributes;
using Parameters;
using Plugins.Ship;
using Plugins.Ship.Sheets;
using Plugins.Ship.Sheets.InfoSheet;
using Plugins.Ship.Sheets.StepSheet;
using Plugins.Ship.Sheets.StepSheet.Commands;
using Plugins.Ship.Sheets.StepSheet.Commands.DefaultCommands;
using Plugins.Ship.Sheets.StepSheet.Steps;
using Plugins.Ship.States;
using Realization.InfoSets;
using Realization.States.CharacterSheet;
using Realization.States.EmptySheetHelper;
using Realization.TutorialRealization.Commands;
using Realization.TutorialRealization.Helpers;
using UnityEngine;
using Constants = Realization.Configs.Constants;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Realization.States
{
    public class StateInitializer : MonoBehaviour
    {
        [SerializeField] private StateConfigurator _stateConfigurator;
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private DungeonConfig _dungeonConfig;
        [SerializeField] private GeneralLevelingConfig _generalLevelingConfig;
        [SerializeField] private ClassLevelingUpConfig _classLevelingUp;

        private ISheet _tutorial;
        private UnityConditions _unityConditions;
        private IState _state;
        private bool _initialized = false;
        private List<Character> _characters = new();
        private List<CharacterStore> _characterStore = new();
        private List<PriorityConfig> _priorities = new();
        private List<Constants> _constants = new();
        private List<ConfigBiom> _dungeons = new();
        private List<StoreCharacterChancesConfig> _storeCharacterChances = new();
        private List<CharacterSet> _characterSets = new();
        private List<LootingConfig> _lootingConfigs;
        private List<ItemLibraryConfig> _itemLibraryConfigs = new();
        private List<Skill> _skills = new();
        private List<GeneralLevelingUpConfig>  _generalLevelingUp;
        private List<TutorialNode> _tutorialNodes;
        private List<LevelingUpConfig>  _levelingUpConfigs;

        private void Awake()
        {
            
            if (_initialized == false)
                InitializeInternal();
            DontDestroyOnLoad(gameObject);
        }

       public void UpdateState()
        {
            // if (_state is { Working: true })
            //     _state?.Update();
        }

        public void InitializeInternal()
        {
            _tutorialNodes = new List<TutorialNode>();
            var loader = MakeTutorialSheet();
            var configLoader = MakeCharactersSheet();
            var constantsLoader = MakeConstantsSheet();
            var configStoreLoader = MakeCharactersStoreSheet();
            var priorityLoader = MakePrioritySheet();
            var dungeonsLoader = MakeDungeonsSheet();
            var storeCharactersChancesLoader = MakeStoreCharactersChancesSheet();
            var characterSetsLoader = MakeCharacterSetsSheet();
            var itemsLibrarySheetLoader = MakeItemsLibrarySheet();
            var skillsSheetLoader = MakeSkillsSheet();
            var lootingSheet = MakeLootingSheet();
            var generalLevelingUpSheet = GeneralLevelingUpSheet();
            var classLevelSheet = MakeСlassLevelingUpSheet();

            var setup =
                    new Setup(_stateConfigurator.StateLink,
                            loader, configLoader, constantsLoader,
                            configStoreLoader, priorityLoader, dungeonsLoader,
                            storeCharactersChancesLoader, characterSetsLoader, itemsLibrarySheetLoader,
                            skillsSheetLoader, lootingSheet,generalLevelingUpSheet,classLevelSheet);

            var stateFactory = new StateFactory(setup);

            _state = stateFactory.Get(_stateConfigurator.CurrentState);
            _state.Start();
            _characterConfig.UpdateCharacters(_characters.ToArray());
            _characterConfig.UpdateCharacterStore(_characterStore.ToArray());
            _characterConfig.UpdatePriorities(_priorities.ToArray());
            _characterConfig.UpdateConstants(_constants[0]);
            _characterConfig.UpdateStoreСharactersСhances(_storeCharacterChances);
            _characterConfig.UpdateCharacterSets(_characterSets);
            _characterConfig.UpdateItemLibrary(_itemLibraryConfigs);
            _characterConfig.UpdateSkills(_skills);
            _characterConfig.UpdateTutorial(_tutorialNodes);

            _dungeonConfig.UpdateLooting(_lootingConfigs);
            List<BiomeData> dungeonConfigBiomeDatas = CreateBioms();
            _dungeonConfig.BiomeDatas = dungeonConfigBiomeDatas;
            _classLevelingUp.UpdateConfig(_levelingUpConfigs);
            _generalLevelingConfig.UpdateConfig(_generalLevelingUp);
            _initialized = true;
        }
        private ISheetLoader MakeСlassLevelingUpSheet()
        {
            _levelingUpConfigs = new List<LevelingUpConfig>();
            var infoBuilder = new ClassLevelingUpBuilder();
            var link = _stateConfigurator.СlassLevelingUP;
            var configLoader =
                    new InfoLoaderFactory<LevelingUpConfig>(link, _stateConfigurator.Postfixes[link], infoBuilder,
                                    _levelingUpConfigs, 1)
                            .Get();
            return configLoader;
        }
        private List<BiomeData> CreateBioms()
        {
            Dictionary<string, BiomeData> biomeDatas = new Dictionary<string, BiomeData>();
            foreach (var dungeon in _dungeons)
            {
                if (biomeDatas.ContainsKey(dungeon.BiomName))
                    biomeDatas[dungeon.BiomName].Configs.Add(dungeon);
                else
                {
                    biomeDatas.Add(dungeon.BiomName, new BiomeData());
                    biomeDatas[dungeon.BiomName].Configs.Add(dungeon);
                    
                    string output = string.Concat(dungeon.BiomName.Where( Char.IsDigit ) );
                    int.TryParse(output, out int key);
                    biomeDatas[dungeon.BiomName].Key = key;
                    biomeDatas[dungeon.BiomName].Name = dungeon.BiomName;
                    biomeDatas[dungeon.BiomName].PrefabName = dungeon.PrefabName;
                }
            }


            List<BiomeData> dungeonConfigBiomeDatas = biomeDatas.Values.ToList();
            return dungeonConfigBiomeDatas;
        }

        private ISheetLoader MakeSkillsSheet()
        {
            _skills = new List<Skill>();
            var infoBuilder = new SkillInfoBuilder();
            var link = _stateConfigurator.SkillsLink;
            var configLoader =
                    new InfoLoaderFactory<Skill>(link, _stateConfigurator.Postfixes[link], infoBuilder,
                                    _skills)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeItemsLibrarySheet()
        {
            _itemLibraryConfigs = new List<ItemLibraryConfig>();
            var infoBuilder = new ItemLibraryBuilder();
            var link = _stateConfigurator.ItemsLibraryLink;
            var configLoader =
                    new InfoLoaderFactory<ItemLibraryConfig>(link, _stateConfigurator.Postfixes[link], infoBuilder,
                                    _itemLibraryConfigs, 2)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeLootingSheet()
        {
            _lootingConfigs = new List<LootingConfig>();
            var lootingBuilder = new LootingBuilder();
            var link = _stateConfigurator.LootingLink;
            var configLoader =
                    new InfoLoaderFactory<LootingConfig>(link, _stateConfigurator.Postfixes[link], lootingBuilder,
                                    _lootingConfigs, 1)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeCharacterSetsSheet()
        {
            _characterSets = new List<CharacterSet>();
            var infoBuilder = new CharacterSetsInfoBuilder();
            var link = _stateConfigurator.CharacterSetsLink;
            var configLoader =
                    new CharacterSetsLoaderFactory
                            (link, _stateConfigurator.Postfixes[link], infoBuilder, _characterSets,
                                    0, -1, true)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeStoreCharactersChancesSheet()
        {
            _storeCharacterChances = new();
            var infoBuilder = new StoreCharacterChancesInfoBuilder();
            var link = _stateConfigurator.StoreCharactersChancesLink;
            var configLoader =
                    new InfoLoaderFactory<StoreCharacterChancesConfig>
                            (link, _stateConfigurator.Postfixes[link], infoBuilder, _storeCharacterChances,
                                    0, -1, true)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeDungeonsSheet()
        {
            _dungeons = new();
            var infoBuilder = new DungeonsConfigInfoBuilder();
            var link = _stateConfigurator.DungeonsLink;
            var configLoader =
                    new InfoLoaderFactory<ConfigBiom>(link, _stateConfigurator.Postfixes[link], infoBuilder,
                                    _dungeons, 2)
                            .Get();
            return configLoader;
        }
        private ISheetLoader GeneralLevelingUpSheet()
        {
            _generalLevelingUp = new();
            var infoBuilder = new GeneralLevelingUpBuilder();
            var link = _stateConfigurator.GeneralLevelingUPLink;
            var configLoader =
                    new InfoLoaderFactory<GeneralLevelingUpConfig>(link, _stateConfigurator.Postfixes[link], infoBuilder,
                                    _generalLevelingUp, 1)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeConstantsSheet()
        {
            _constants = new List<Constants>();
            var infoBuilder = new ConstantsInfoBuilder();
            var link = _stateConfigurator.ConstantsLink;
            var configLoader =
                    new InfoLoaderFactory<Constants>(link, _stateConfigurator.Postfixes[link],
                                    infoBuilder, _constants, 1, -1, true)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeCharactersStoreSheet()
        {
            var infoBuilder = new CharacterStoreInfoBuilder();
            var configLoader =
                    new InfoLoaderFactory<CharacterStore>(
                            _stateConfigurator.CharactersStoreLink,
                            _stateConfigurator.Postfixes[_stateConfigurator.CharactersStoreLink],
                            infoBuilder, _characterStore, 2).Get();
            return configLoader;
        }

        private ISheetLoader MakePrioritySheet()
        {
            var infoBuilder = new PriorityConfigBuilder();
            var configLoader =
                    new InfoLoaderFactory<PriorityConfig>(_stateConfigurator.PriorityLink,
                                    _stateConfigurator.Postfixes[_stateConfigurator.PriorityLink], infoBuilder,
                                    _priorities)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeCharactersSheet()
        {
            var infoBuilder = new CharacterInfoBuilder();
            var link = _stateConfigurator.CharactersLink;
            var configLoader =
                    new InfoLoaderFactory<Character>(link, _stateConfigurator.Postfixes[link], infoBuilder, _characters)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeTutorialSheet()
        {
            var builder = new TutorialNodeBuilder();
            var link = _stateConfigurator.TutorialLink;
            var loader = new InfoLoaderFactory<TutorialNode>
                    (link, _stateConfigurator.Postfixes[link], builder, _tutorialNodes).Get();
            return loader;
        }
    }
}