using System.Collections.Generic;
using Parameters;
using Realization.Configs;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [CreateAssetMenu(fileName = "New Characters Config", menuName = "Configs/Characters", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        [SerializeField] private List<Character> _characters;
        [SerializeField] private List<CharacterStore> _characterStore;
        [SerializeField] private List<PriorityConfig> _priorities;
        [SerializeField] private List<StoreCharacterChancesConfig> _storeСharactersСhances;
        [SerializeField] private List<CharacterSet> _characterSets;
        [SerializeField] private List<Skill> _skills;
        [SerializeField] private List<ItemLibraryConfig> _itemsTestConfigs;
        [SerializeField] private List<TutorialNode> _tutorial;

        public Character[] Characters
        {
            get => _characters.ToArray();
            set => _characters = new List<Character>(value);
        }

        public CharacterStore[] CharacterStore
        {
            get => _characterStore.ToArray();
            set => _characterStore = new List<CharacterStore>(value);
        }

        public PriorityConfig[] Priorities {
            get => _priorities.ToArray();
            set => _priorities = new List<PriorityConfig>(value);
        }

        public int CurrentCharacterStorePrice
        {
            get
            {
                var index = PlayerPrefs.GetInt("level");
                return CharacterStore[0].Price;
            }
        }

        public List<StoreCharacterChancesConfig> StoreСharactersСhances
        {
            get => _storeСharactersСhances;
            set => _storeСharactersСhances = value;
        }
        
        public List<CharacterSet> CharacterSets
        {
            get
            {
                string text = "";
                Skill skill = _skills[0];
                for (int i = 0; i < _characterSets.Count; i++)
                {
                    if(_characterSets[i].UnitCount == 0)
                    {
                        _characterSets.RemoveAt(i);
                        --i;
                        continue;
                    }
                    skill = _skills.Find(s => s.Uid == _characterSets[i].Effect);

                    _characterSets[i] = new CharacterSet(_characterSets[i], skill);

                }
                return _characterSets;
            }
            set => _characterSets = new List<CharacterSet>(value);
        }

        public Constants Constants;
        public List<TutorialNode> TutorialNodes
        {
            get => _tutorial;
            set => _tutorial = value;
        }

        public List<Skill> Skills
        {
            get => _skills;
            set => _skills = value;
        }

        public List<ItemLibraryConfig> ItemsTestConfigs
        {
            get => _itemsTestConfigs;
            set => _itemsTestConfigs = value;
        }

        public void UpdateCharacters(Character[] characters)
        {
            _characters = new List<Character>(characters);
            _characters.RemoveAll((character => character.Uid == ""));
        }
        
        public void UpdateCharacterStore(CharacterStore[] characters)
        {
            _characterStore = new List<CharacterStore>(characters);
        }

        public void UpdateConstants(Constants constant)
        {
            Constants = constant;
        }

        public void UpdatePriorities(PriorityConfig[] priorityConfigs)
        {
            _priorities = new List<PriorityConfig>(priorityConfigs);
        }

        public void UpdateStoreСharactersСhances(List<StoreCharacterChancesConfig> storeCharacterChances)
        {
            StoreСharactersСhances = new List<StoreCharacterChancesConfig>(storeCharacterChances);
        }

        public void UpdateCharacterSets(List<CharacterSet> characterSets)
        {
            CharacterSets = new List<CharacterSet>(characterSets);
        }

        public void UpdateItemLibrary(List<ItemLibraryConfig> itemsTestConfigs)
        {
            ItemsTestConfigs = new List<ItemLibraryConfig>(itemsTestConfigs);
        }

        public void UpdateSkills(List<Skill> skills)
        {
            Skills = new List<Skill>(skills);
        }

        public void UpdateTutorial(List<TutorialNode> tutorialSteps)
        {
            TutorialNodes = tutorialSteps;
        }
    }
}