using System.Collections.Generic;
using System.Linq;
using Fight.Fractions;
using Grids;
using Infrastructure.Shared.Extensions;
using Realization.States.CharacterSheet;
using Realization.UnitplaceSets;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.General
{
    public class UnitFactory : MonoBehaviour // TODO: rename and remake
    {
        private const int RandomStartCharacter = -1;
        private const int NoItem = -2;

        [SerializeField] private bool _spawnRandom = true;
        [SerializeField] private bool _spawnAtStart = true;
        [SerializeField] private int _minionsAtStart = -1;

        private CharacterConfig _characterConfig;
        private MinionFactory _factory;
        private IGrid<IMinion> _grid;

        [Inject]
        private void Construct(MinionFactory factory, IGrid<IMinion> gridBehaviour, CharacterConfig characterConfig)
        {

            _factory = factory;
            _grid = gridBehaviour;
            _characterConfig = characterConfig;

            
            /*
            if (PlayerPrefs.GetInt("level") == 1)
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.SetInt("level", 1);
            }*/
            
        }

        private void Start()
        {
            if (_spawnAtStart == false)
                return;
            
            int count = _minionsAtStart;

            if(_minionsAtStart == -1)
            {
                count = PlayerPrefs.GetInt("UnitCount", 0);
            }

            var minions = new List<IMinion>();

            for (int i = 0; i < count; i++)
            {
                int charIndex = _spawnRandom ? RandomStartCharacter : i;
                minions.Add(CreateUnit(charIndex, charIndex));
            }
            var placer = new AutoPlace(_grid, false);
            placer.PlaceMinions(minions.ToArray()); 
        }

        private IMinion CreateUnit(int characterIndex, int index = -1)
        {
            Character character = CreateNewCharacter(characterIndex);
            IMinion minion = _factory.CreateAndReturn(character);
            LoadChild(minion, index);
            return minion;
        }
        
        public void Save()
        {
            int index = 0;

            foreach (var minion in _factory.Minions)
            {
                if (minion.Fraction == Fight.Fractions.Fraction.Minions)
                {
                    SaveChild(minion, index);
                    index++;
                }
            }

            PlayerPrefs.SetInt("UnitCount", index);
        }

        private void SaveChild(IMinion minion, int index)
        {
            if (minion.Parameters.Health.Value <= 0)
                return;

            var gridCoordinates = minion.Position;
            
            PlayerPrefs.SetFloat($"minion_gridPositionX_{index}", gridCoordinates.x);
            PlayerPrefs.SetFloat($"minion_gridPositionY_{index}", gridCoordinates.y);
            PlayerPrefs.SetFloat($"minion_hp_{index}", minion.Parameters.Health.Value);

            PlayerPrefs.SetString($"minion_uid_{index}", minion.Uid);
            Debug.Log(index + " " + PlayerPrefs.GetString("minion_uid_" + index));
        }

        private void LoadChild(IMinion minion, int index = -1)
        {
            if (PlayerPrefs.GetInt("AdsWatched", 0) == 0)
            {
                float hp = PlayerPrefs.GetFloat($"minion_hp_{index}", minion.Parameters.Health.Value);
                float delta = minion.Parameters.Health.Value - hp;
                minion.Parameters.Health.Decrease((int)Mathf.Abs(delta));
                PlayerPrefs.SetInt("AdsWatched", 0);
            }
        }

        private Character CreateNewCharacter(int itemIndex)
        {
            Character item;
            Character character;

            if (itemIndex == RandomStartCharacter || itemIndex == NoItem)
            {
                item = _characterConfig.Characters
                    .Where(character1 => character1.Tags != null && character1.Tags.Contains("ally"))
                    .Where(character2 => character2.Grade == 1)
                    .Random();
            }
            else
            {
                Debug.Log(itemIndex + " " + PlayerPrefs.GetString("minion_uid_" + itemIndex));
                string uid = PlayerPrefs.GetString("minion_uid_" + itemIndex);
                item = _characterConfig.Characters.FirstOrDefault(character => character.Uid.Equals(uid)) ??
                       _characterConfig.Characters
                    .Where(character1 => character1.Tags != null && character1.Tags.Contains("ally"))
                    .Where(character2 => character2.Grade == 1)
                    .Random();
            }

            character = new Character()
            {
                Uid = item.Uid,
                Class = item.Class,
                Prefab = item.Prefab,
                Grade = item.Grade,
                Tags = item.Tags,
                Health = item.Health,
                Armor = item.Armor,
                Power = item.Power,
                TimeBetweenAttacks = item.TimeBetweenAttacks,
                Range = item.Range,
                CriticalDamageChance = item.CriticalDamageChance,
                CriticalDamageMultiplier = item.CriticalDamageMultiplier,
                ChanceOfDodge = item.ChanceOfDodge,
                Energy = item.Energy,
                PowerOfHealing = item.PowerOfHealing,
                AdditionalParameter = item.AdditionalParameter,
                Skill = item.Skill
            };
            return character;
        }
    }
}