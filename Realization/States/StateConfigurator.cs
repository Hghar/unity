using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NaughtyAttributes;
using Newtonsoft.Json;
using Parameters;
using Plugins.Ship;
using Plugins.Ship.Saver;
using Plugins.Ship.Sheets;
using Plugins.Ship.Sheets.InfoSheet;
using Plugins.Ship.Sheets.StepSheet;
using Plugins.Ship.Sheets.StepSheet.Commands;
using Plugins.Ship.Sheets.StepSheet.Commands.DefaultCommands;
using Plugins.Ship.Sheets.StepSheet.Steps;
using Plugins.Ship.States;
using Realization.Configs;
using Realization.InfoSets;
using Realization.States.CharacterSheet;
using Realization.States.EmptySheetHelper;
using UnityEngine;
using Constants = Realization.Configs.Constants;

namespace Realization.States
{
    [CreateAssetMenu(fileName = "New State Configurator", menuName = "States/Configurator", order = 0)]
    public class StateConfigurator : ScriptableObject
    {
        [HideInInspector] [SerializeField] private List<State> _states = new();

        private string[] _names => _states.Select((state => state.Name)).ToArray();
        [OnValueChanged("CurrentStateChangeCallback")]
        [Dropdown("_names")] [SerializeField] private string _currentState = "none";

        [HorizontalLine(color: EColor.White)]
        [InfoBox("Links for connecting to Google Sheets.", EInfoBoxType.Normal)]
        [Header("Links")]
        [SerializeField]
        private string _tutorialLink = "1it21EgmUDHjdkw6ykFzq_fTq0jqJwFJ52MMUQOrV6JI";

        [SerializeField] private string _charactersLink = "1D8AYX3Kjb2ozAxZ8lkJNO41HDmtC48pxI_Rr8LygOBs";
        [SerializeField] private string _charactersStoreLink = "18gtv8Pj_fKN1NCnzu_jmwE_iulyuna4lI-zE4VAwVqY";
        [SerializeField] private string _constantsLink = "1YOLIQF0_m6fUl3nROKbsPxql86BoAnh1vTAlawJ0daw";
        [SerializeField] private string _stateLink = "1pSHyzUUO40FQqUBb3QsxIpIpCSkSQMLpkflimBJ-ClE";
        [SerializeField] private string _priorityLink = "1d_OkTaJqxPIGXXVCjpzjRXhFqXlO5UU5arjgWlGoXbM";
        [SerializeField] private string _dungeonsLink = "1Zka_zNjP6otAzzbjZaX3UPjsvXefVL63FOChS85BlhY";
        [SerializeField] private string _storeCharactersChancesLink = "1alX5AEAIFgMgcVCeHH44zzhK4PCS_-2X6EwpM7q10KM";
        [SerializeField] private string _characterSetsLink = "1tFitbh8XNr5pRwOwuf6UE94bZHYYJ-V-1gp0BQJnI2c";
        [SerializeField] private string _lootingLink = "1LG0NRdV68KC5ABb3odn4n3xFSOGqqBpMZzteQfDm3rk";
        [SerializeField] private string _itemsLibraryLink = "16XNx7Zo7tKfGI1ftwDqK_pbYpGNxP5nz881Z_QSgmVg";
        [SerializeField] private string _skillsLink = "1GfZ175rEDAFeDROcfpUS5pPPRcLwahqR10kLG9l1L98";
        [SerializeField] private string _generalLevelingUP = "1OBSR27NQyGk3R9G57v8_1VlqDyxeNeKPtjVlNQHN0PA";
        [SerializeField] private string _classLevelingUP = "13uu7drZ6LmS3esILKw13ryYPIo7bTF0IkrKLotMmP-I";

        private string[] _sheetLinks = new[] {"Empty"};

        [BoxGroup("Sheet downloader")]
        [InfoBox("Field to download to the game only one sheet from Google Sheets instead of all. " +
                 "Need to select a sheet and press 'DownloadSelectedSheet'." +
                 "The sheets not liked to a special state, so if you update for example " +
                 "tutorial sheet 'main', it will be updated on every state that use it.", EInfoBoxType.Normal)]
        [Dropdown("_sheetLinks")]
        [SerializeField]
        private string _selectedSheet = "none";

        private string[] _stateLinks = new[] {"Empty"};

        [BoxGroup("States downloader")]
        [InfoBox("Field to download to the game only one state from Google Sheets instead of all. " +
                 "Need to select a sheet and press 'DownloadSelectedState'." +
                 "It will update every sheet that used in the chosen state.", EInfoBoxType.Normal)]
        [Dropdown("_stateLinks")]
        [SerializeField]
        private string _selectedState = "none";

        [InfoBox("If the fields 'Selected Sheet' and 'Selected State' are empty then need to press 'Download' " +
                 "and update all states and sheets.", EInfoBoxType.Warning)]
        [Label("")]
        [ReadOnly]
        [SerializeField]
        private string _tip;

        [SerializeField] private List<PostfixPair> _postfixes = new();
        [HideInInspector]
        [SerializeField] private SaveFile _save;


        public string CurrentState => _save.CurrentState;

        public string TutorialLink => _tutorialLink;
        public string ConstantsLink => _constantsLink;
        public string CharactersLink => _charactersLink;
        public string StateLink => _stateLink;
        public Dictionary<string, string> Postfixes => PostfixesToDictionary();

        public string CharactersStoreLink => _charactersStoreLink;

        public string PriorityLink => _priorityLink;
        public string DungeonsLink => _dungeonsLink;
        public string GeneralLevelingUPLink => _generalLevelingUP;
        public string СlassLevelingUP => _classLevelingUP;

        public string StoreCharactersChancesLink
        {
            get => _storeCharactersChancesLink;
            set => _storeCharactersChancesLink = value;
        }

        public string CharacterSetsLink => _characterSetsLink;
        public string LootingLink => _lootingLink;

        public string SkillsLink => _skillsLink;

        public string ItemsLibraryLink
        {
            get => _itemsLibraryLink;
            set => _itemsLibraryLink = value;
        }

        private Dictionary<string, string> PostfixesToDictionary()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var postfix in _postfixes)
            {
                dictionary.TryAdd(postfix.Link, postfix.Postfix);
            }

            return dictionary;
        }
        [SerializeField] public bool Updated = false;

        [Button("Download selected sheet")]
        private async void DownloadSelectedSheet()
        {
            // string patternName = @"(?=_\w*$)(\w+)";
            // Regex regexName = new Regex(patternName);
            // Match matchName = regexName.Match(_selectedSheet);

            SaveFile save = Resources.Load<SaveFile>($"DefaultSave");
            string sheet = save.Saves.Find((s => s.Name == _selectedSheet)).Link;

            var name = _selectedSheet;
            var postfix = "";
            foreach (var postfixPair in _postfixes)
            {
                if(name.Contains(postfixPair.Postfix))
                {
                    name = name.Replace(postfixPair.Postfix, "");
                    postfix = postfixPair.Postfix;
                    break;
                }
            }
            name = name.Substring(0, name.Length - 1);
            
            var objs = await SaveAndLoad.DownloadSheet(sheet, 
                name);
            string downloadedSheet = JsonConvert.SerializeObject(objs, Formatting.Indented);
            
            await SaveAndLoad.Save(sheet, name, postfix, downloadedSheet);
            
            Updated = true;
            Debug.Log($"Selected sheet downloaded successfully ({name} _ {postfix})");
        }

        [Button("Download selected state")]
        private async void DownloadSelectedState()
        {
            string patternName = @"\w+(?=_\w*$)";
            Regex regexName = new Regex(patternName);
            Match matchName = regexName.Match(_selectedState);

            var tutorial = MakeTutorialSheet();
            var configLoader = MakeCharactersSheet();
            var constantsLoader = MakeConstantsSheet();
            var configStoreLoader = MakeCharactersStoreSheet();
            var priorityLoader = MakePrioritySheet();
            var dungeonsLoader = MakeDungeonsSheet();
            var storeCharactersChancesLoader = MakeStoreCharactersChancesSheet();
            var characterSetsLoader = MakeCharacterSetsSheet();
            var itemsLibraryLoader = MakeItemsLibrarySheet();
            var skillsSheetLoader = MakeSkillsSheet();
            var lootingSheet = MakeLootingSheet();
            var generalLevelingUpLink = MakeGeneralLevelingUpSheet();
            var classLevelSheet = MakeСlassLevelingUpSheet();

            var setup = new Setup(StateLink, 
                tutorial, configLoader, constantsLoader, configStoreLoader, priorityLoader, dungeonsLoader,
                storeCharactersChancesLoader, characterSetsLoader, itemsLibraryLoader, skillsSheetLoader,
                lootingSheet,generalLevelingUpLink,classLevelSheet);

            var stateFactory = new StateFactory(setup);

            await stateFactory.Save(matchName.Value);

            Updated = true;
            Debug.Log($"Selected state downloaded successfully");
        }

        [Button]
        private async void Download()
        {
            var saves = Resources.Load<SaveFile>($"DefaultSave").Saves;
            Postfixes.Clear();
            foreach (var save in saves)
            {
                string patternPostfix = @"([^_]+)$";
                Regex regexPostfix = new Regex(patternPostfix);
                Match matchPostfix = regexPostfix.Match(save.Name);
                if (_postfixes.Any(s => s.Link == save.Link) == false)
                    _postfixes.Add(new PostfixPair(save.Link, matchPostfix.Value));
            }
            
            IList<IList<object>> loaded = 
                await SaveAndLoad.DownloadSheet(StateLink, "States");
            _states.Clear();
            //row with name
            loaded.RemoveAt(0);
            foreach (IList<object> row in loaded)
            {
                string stateRowValue = row[0] as string;
                _states.Add(new State(stateRowValue));
                
            }

            var tutorial = MakeTutorialSheet();
            var configLoader = MakeCharactersSheet();
            var constantsLoader = MakeConstantsSheet();
            var configStoreLoader = MakeCharactersStoreSheet();
            var priorityLoader = MakePrioritySheet();
            var dungeonsLoader = MakeDungeonsSheet();
            var storeCharactersChancesLoader = MakeStoreCharactersChancesSheet();
            var characterSetsLoader = MakeCharacterSetsSheet();
            var itemsLibraryLoader = MakeItemsLibrarySheet();
            var skillsSheetLoader = MakeSkillsSheet();
            var lootingSheet = MakeLootingSheet();
            var generalLevelingUpLink = MakeGeneralLevelingUpSheet();
            var classLevelSheet = MakeСlassLevelingUpSheet();


            var setup = new Setup(StateLink, 
                            tutorial, configLoader, constantsLoader, configStoreLoader, priorityLoader, dungeonsLoader,
                            storeCharactersChancesLoader, characterSetsLoader,itemsLibraryLoader, skillsSheetLoader,
                            lootingSheet,generalLevelingUpLink,classLevelSheet);

            var stateFactory = new StateFactory(setup);

            await stateFactory.SaveAll();

            _sheetLinks = saves
                .Where((save => save.Link != StateLink))
                .Select((state => state.Name)).ToArray();
            _stateLinks = saves
                .Where((save => save.Link == StateLink))
                .Select((state => state.Name)).ToArray();

            Updated = true;
            Debug.Log($"All sheets and states downloaded successfully");
        }
        
        private ISheetLoader MakeSkillsSheet()
        {
            var skills = new List<Skill>();
            var infoBuilder = new SkillInfoBuilder();
            var link = SkillsLink;
            var configLoader =
                new InfoLoaderFactory<Skill>(link, Postfixes[link], infoBuilder,
                        skills)
                    .Get();
            return configLoader;
        }

        private ISheetLoader MakeCharacterSetsSheet()
        {
            var sets  = new List<CharacterSet>();
            var infoBuilder = new CharacterSetsInfoBuilder();
            var link = CharacterSetsLink;
            var configLoader =
                new CharacterSetsLoaderFactory
                    (link, Postfixes[link], infoBuilder, sets, 
                        0, -1, true)
                    .Get();
            return configLoader;
        }

        private ISheetLoader MakeStoreCharactersChancesSheet()
        {
            var chances  = new List<StoreCharacterChancesConfig>();
            var infoBuilder = new StoreCharacterChancesInfoBuilder();
            var link = StoreCharactersChancesLink;
            var configLoader =
                new InfoLoaderFactory<StoreCharacterChancesConfig>
                    (link, Postfixes[link], infoBuilder, chances, 
                        0, -1, true)
                    .Get();
            return configLoader;
        }
        
        private ISheetLoader MakeItemsLibrarySheet()
        {
            var dungeons  = new List<ItemLibraryConfig>();
            var infoBuilder = new ItemLibraryBuilder();
            var link =ItemsLibraryLink ;
            var configLoader =
                    new InfoLoaderFactory<ItemLibraryConfig>(link, Postfixes[link], infoBuilder, dungeons, 2)
                            .Get();
            return configLoader;
        }

        private ISheetLoader MakeLootingSheet()
        {
            var lootingConfigs = new List<LootingConfig>();
            var infoBuilder = new LootingBuilder();
            var link = LootingLink;
            var configLoader =
                    new InfoLoaderFactory<LootingConfig>(link, Postfixes[link], infoBuilder, lootingConfigs, 1)
                            .Get();
            return configLoader;
        }
        
        private ISheetLoader MakeGeneralLevelingUpSheet()
        {
            var generalLevelingUp = new List<GeneralLevelingUpConfig>();
            var infoBuilder = new GeneralLevelingUpBuilder();
            var link = GeneralLevelingUPLink;
            var configLoader =
                    new InfoLoaderFactory<GeneralLevelingUpConfig>(link, Postfixes[link], infoBuilder,
                                    generalLevelingUp, 1)
                            .Get();
            return configLoader;
        }
        private ISheetLoader MakeСlassLevelingUpSheet()
        {
            var generalLevelingUp = new List<LevelingUpConfig>();
            var infoBuilder = new ClassLevelingUpBuilder();
            var link = СlassLevelingUP;
            var configLoader =
                    new InfoLoaderFactory<LevelingUpConfig>(link, Postfixes[link], infoBuilder,
                                    generalLevelingUp, 1)
                            .Get();
            return configLoader;
        }
        private ISheetLoader MakeDungeonsSheet()
        {
            var dungeons  = new List<ConfigBiom>();
            var infoBuilder = new DungeonsConfigInfoBuilder();
            var link = DungeonsLink;
            var configLoader =
                new InfoLoaderFactory<ConfigBiom>(link, Postfixes[link], infoBuilder, dungeons, 2)
                    .Get();
            return configLoader;
        }

        private ISheetLoader MakeCharactersSheet()
        {
            var players = new List<Character>();
            var infoBuilder = new CharacterInfoBuilder();
            var configLoader =
                new InfoLoaderFactory<Character>(
                    CharactersLink, 
                    Postfixes[CharactersLink], 
                    infoBuilder, 
                    players, 
                    2).Get();
            return configLoader;
        }
        
        private ISheetLoader MakeCharactersStoreSheet()
        {
            var players = new List<Parameters.CharacterStore>();
            var infoBuilder = new CharacterStoreInfoBuilder();
            var configLoader =
                new InfoLoaderFactory<Parameters.CharacterStore>(CharactersStoreLink, Postfixes[CharactersStoreLink], infoBuilder, players).Get();
            return configLoader;
        }

        private ISheetLoader MakeConstantsSheet()
        {
            var constants = new List<Constants>();
            var infoBuilder = new ConstantsInfoBuilder();
            var configLoader =
                new InfoLoaderFactory<Constants>(ConstantsLink, Postfixes[ConstantsLink], infoBuilder, constants, 
                    1, -1, true).Get();
            return configLoader;
        }
        
        private ISheetLoader MakePrioritySheet()
        {
            var priorities = new List<PriorityConfig>();
            var infoBuilder = new PriorityConfigBuilder();
            var configLoader =
                new InfoLoaderFactory<PriorityConfig>(PriorityLink, Postfixes[PriorityLink], infoBuilder, priorities).Get();
            return configLoader;
        }

        private ISheetLoader MakeTutorialSheet()
        {
            IConditionDictionary[] conditions =
            {
                new DefaultConditions()
            };
            IActionDictionary[] actions =
            {
                new DefaultActions()
            };

            var nameConverter = new NameConverter(conditions, actions);
            var builder = new NodeBuilder(nameConverter);
            var loader = new StepLoaderFactory(TutorialLink, Postfixes[TutorialLink], builder).Get();
            return loader;
        }

        [Button("Validate (not ready)")]
        private void Validate()
        {
            //todo
        }

        private void CurrentStateChangeCallback()
        {
            if(_save == null)
                _save = Resources.Load<SaveFile>($"DefaultSave");
            
            _save.CurrentState = _currentState;
            Updated = true;
        }
    }

    [Serializable]
    public struct PostfixPair
    {
        public string Link;
        public string Postfix;

        public PostfixPair(string link, string postfix)
        {
            Link = link;
            Postfix = postfix;
        }
    }
}