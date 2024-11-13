using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Services.SaveLoadService;
using Infrastructure.Services.StaticData;
using Model.Economy;
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
using Realization.States;
using Realization.States.CharacterSheet;
using Realization.TutorialRealization.Commands;
using Realization.TutorialRealization.Helpers;
using UnityEditor;
using UnityEngine;
using Constants = Realization.Configs.Constants;


namespace Editor
{
#pragma warning disable CS0162

    public class Tools
    {
        [MenuItem("Tools/ClearEditorPrefs")]
        public static void ClearEditorPrefs()
        {
           // EditorPrefs.DeleteKey(ConfigsParser.ParserKey);
        }
    }
    public class ParserSittings
    {
        public bool AutoBakeAfterDownload = true;
    }
    /*public class ConfigsParser :  EditorWindow
    {
        [SerializeField] private StateConfigurator _stateConfigurator;
        [SerializeField] private ObjectFinder _objectFinder;
        [SerializeField] private HardTutorial _hardTutorial;
        [SerializeField] private CharacterConfig _characterConfig;
        [SerializeField] private DungeonConfig _dungeonConfig;
        [SerializeField] private GeneralLevelingConfig _generalLevelingConfig;

        private UnityConditions _unityConditions;
        private IState _state;
        private List<Character> _characters = new();
        private List<CharacterStore> _characterStore = new();
        private List<PriorityConfig> _priorities = new();
        private List<Constants> _constants = new();
        private List<DungeonsConfig> _dungeons = new();
        private List<StoreCharacterChancesConfig> _storeCharacterChances = new();
        private List<CharacterSet> _characterSets = new();
        private List<LootingConfig> _lootingConfigs;
        private List<ItemLibraryConfig> _itemLibraryConfigs = new();
        private List<Skill> _skills = new();
        private static bool _startDownload;
        private static bool _autoBake;
        private static ParserSittings _sittings;
        public static  string ParserKey = "Parser";
        private int _selectedState = 0;
        private int _selectedLink;
        private int _selectedSheet;
        private List<TutorialNode> _tutorialNodes;
        private List<GeneralLevelingUpConfig>  _generalLevelingUp;

        private const string k_ProjectOpened = "ProjectOpened";

        [InitializeOnLoadMethod]
        static void AutoInit()
        {
            if (!SessionState.GetBool(k_ProjectOpened, false))
            {
                _sittings = EditorPrefs.GetString(ParserKey)?
                                    .ToDeserialized<ParserSittings>()
                            ?? new ParserSittings();
                _autoBake = _sittings.AutoBakeAfterDownload;

                SessionState.SetBool(k_ProjectOpened, true);
                EditorApplication.delayCall += OpenWindow;
            }
        }

        [MenuItem("AsomGames/Config Baker")]
        static void InitWindow()
        {
            _sittings = EditorPrefs.GetString(ParserKey)?
                                .ToDeserialized<ParserSittings>()
                        ?? new ParserSittings();
            _autoBake = _sittings.AutoBakeAfterDownload;
            CreateWindow();
        }

        private static ConfigsParser CreateWindow()
        {
            ConfigsParser window = (ConfigsParser)GetWindow(typeof(ConfigsParser));
            window.maxSize = new Vector2(300, 600);
            window.minSize = new Vector2(300, 600);
            window.Show();
            return window;
        }

        private void OnDestroy()
        {
            EditorApplication.delayCall -=OpenWindow;
        }

        private static void OpenWindow()
        {
            var window = CreateWindow();
            window.InitStates();
        }

        private void InitStates()
        {
            Debug.Log("<color=red>States run to initialize</color>");
            _stateConfigurator.InitStates();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            string[] strings = _stateConfigurator.States.Select(x => x.Name.ToString()).ToArray();
            _selectedState = EditorGUILayout.Popup("CurrentState", _selectedState, strings);
            _stateConfigurator.SetCurrentState(_selectedState);
            
            GUILayout.Space(10);

            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 15;
            myStyle.normal.textColor = Color.yellow;
            myStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label( "Buttons for initialize gameconfigs !",myStyle);

            if (GUILayout.Button("Bake configs")) Initialize();
            if (GUILayout.Button("Clear Character Config")) ClearCharacterConfig();
            if (GUILayout.Button("Clear Dungeon Config")) ClearDungeonConfig();
           
            if (GUILayout.Button("Clear All configs"))
            {
                ClearCharacterConfig();
                ClearDungeonConfig();
            }
            GUILayout.Space(40);
            
            GUIStyle spreadsheatStyle = new GUIStyle();
            spreadsheatStyle.fontSize = 15;
            spreadsheatStyle.normal.textColor = Color.red;
            spreadsheatStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label( "Buttons for download spreadsheets !",spreadsheatStyle);

          

            if (GUILayout.Button("Download spreadsheets")) 
                _stateConfigurator.Download(BakeConfigs);

            GUILayout.Space(50);

            _selectedLink = EditorGUILayout.Popup("States downloader", _selectedLink, _stateConfigurator.StateLinks);
            _stateConfigurator.SetSelectedLink(_selectedLink);
            if (GUILayout.Button("Download selected state")) 
                _stateConfigurator.DownloadSelectedState(BakeConfigs);

            GUILayout.Space(50);

            _selectedSheet = EditorGUILayout.Popup("Sheet downloader", _selectedSheet, _stateConfigurator.SheetLinks);
            _stateConfigurator.SetSelectedSheet(_selectedSheet);
            if (GUILayout.Button("Download selected sheet")) 
                _stateConfigurator.DownloadSelectedSheet(BakeConfigs);
        
           
            GUILayout.Space(50);


            GUIStyle sittings = new GUIStyle();
            sittings.fontSize = 15;
            sittings.normal.textColor = Color.white;
            sittings.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label( "Exstra !",sittings);

            GUILayout.Space(10);
            _autoBake = GUILayout.Toggle(_autoBake, "AutoBake after download");
            if (GUILayout.Button("Save Sittings"))
                SaveSittings();
            if (GUILayout.Button("InitStates")) 
                _stateConfigurator.InitStates();
        }

        private void SaveSittings()
        {
            _sittings.AutoBakeAfterDownload = _autoBake;
            EditorPrefs.SetString(ParserKey, _sittings.ToJson());
        }

        private void BakeConfigs()
        {
            if (_sittings.AutoBakeAfterDownload)
                Initialize();
        }

        private void ClearCharacterConfig()
        {
            _characterConfig.Characters = new Character[] { };
            _characterConfig.CharacterStore = new CharacterStore[] { };
            _characterConfig.Constants = new Constants();
            _characterConfig.Skills = new List<Skill>();
            _characterConfig.StoreСharactersСhances = new List<StoreCharacterChancesConfig>();
            _characterConfig.ItemsTestConfigs = new List<ItemLibraryConfig>();
            _characterConfig.CharacterSets = new List<CharacterSet>();
            _characterConfig.Priorities = new PriorityConfig[] { };
            _characterConfig.TutorialNodes = new List<TutorialNode>();
        }

        private void ClearDungeonConfig()
        {
            _dungeonConfig.Dungeons = new List<DungeonsConfig>();
            _dungeonConfig.Looting = new List<LootingConfig>();
            _dungeonConfig.BiomeDatas = new List<BiomeData>();
        }

        private void Update()
        {
            UpdateState();
        }

        private void Initialize()
        {
           InitializeInternal();
        }

        private void UpdateState()
        {
            if (_state is { Working: true }) 
                _state?.Update();
        }

        private void InitializeInternal()
        {
            _tutorialNodes = new List<TutorialNode>();
            _characters = new List<Character>();
            _characterStore = new List<CharacterStore>();
            _priorities = new List<PriorityConfig>();
            _storeCharacterChances = new List<StoreCharacterChancesConfig>();
            _constants = new List<Constants>();
            _skills = new List<Skill>();
            _itemLibraryConfigs = new List<ItemLibraryConfig>();
            _dungeons = new List<DungeonsConfig>();
            _lootingConfigs = new List<LootingConfig>();
            
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

            var setup =
                    new Setup(_stateConfigurator.StateLink,
                            loader, configLoader, constantsLoader,
                            configStoreLoader, priorityLoader, dungeonsLoader,
                            storeCharactersChancesLoader, characterSetsLoader, itemsLibrarySheetLoader,
                            skillsSheetLoader, lootingSheet, generalLevelingUpSheet);

            var stateFactory = new StateFactory(setup);

            _state = stateFactory.Get(_stateConfigurator.CurrentState);
            _state.Start();
            _characterConfig.UpdateTutorial(_tutorialNodes);
            _characterConfig.UpdateCharacters(_characters.ToArray());
            _characterConfig.UpdateCharacterStore(_characterStore.ToArray());
            _characterConfig.UpdatePriorities(_priorities.ToArray());
            _characterConfig.UpdateConstants(_constants[0]);
            _characterConfig.UpdateStoreСharactersСhances(_storeCharacterChances);
            _characterConfig.UpdateCharacterSets(_characterSets);
            _characterConfig.UpdateItemLibrary(_itemLibraryConfigs);
            _characterConfig.UpdateSkills(_skills);

            _dungeonConfig.UpdateLooting(_lootingConfigs);
            _dungeonConfig.Dungeons = _dungeons;
            List<BiomeData> dungeonConfigBiomeDatas = CreateBioms();
            _dungeonConfig.BiomeDatas = dungeonConfigBiomeDatas;
            _generalLevelingConfig.UpdateConfig(_generalLevelingUp);
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
                    new InfoLoaderFactory<DungeonsConfig>(link, _stateConfigurator.Postfixes[link], infoBuilder,
                                    _dungeons, 2)
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
            // _unityConditions = new UnityConditions();
            // IConditionDictionary[] conditions =
            // {
            //         new DefaultConditions(),
            //         _unityConditions,
            // };
            // IActionDictionary[] actions =
            // {
            //         new DefaultActions(),
            //         new UnityActions(_objectFinder, _hardTutorial),
            // };
            // var nameConverter = new NameConverter(conditions, actions);
            // var builder = new NodeBuilder(nameConverter);
            var builder = new TutorialNodeBuilder();
            var link = _stateConfigurator.TutorialLink;
            var loader = new InfoLoaderFactory<TutorialNode>
                    (link, _stateConfigurator.Postfixes[link], builder, _tutorialNodes).Get();
            return loader;
        }
    }*/
}