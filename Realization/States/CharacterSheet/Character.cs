using System;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;
using Units;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [Serializable]
    public class Character : IInfo<Character>, IReadOnlyCharacter
    {
        [SerializeField] private string _uid;
        [SerializeField] private MinionClass _class;
        [SerializeField] private string _prefab;
        [SerializeField] private int _grade;
        [SerializeField] private string _tags;
        [SerializeField] private int _health;
        [SerializeField] private float _armor;
        [SerializeField] private float _power;
        [SerializeField] private float _timeBetweenAttacks;
        [SerializeField] private int _range;
        [SerializeField] private float _criticalDamageChance;
        [SerializeField] private float _criticalDamageMultiplier;
        [SerializeField] private float _chanceOfDodge;
        [SerializeField] private int _energy;
        [SerializeField] private int _powerOfHealing;
        [SerializeField] private float _additionalParameter;
        [SerializeField] private string _skill;
        [SerializeField] private int _startLevel = 1;

        [SerializeField] private PriorityConfig _priorityConfig;

        public string Uid
        {
            get => _uid;
            set => _uid = value;
        }

        public MinionClass Class
        {
            get => _class;
            set => _class = value;
        }

        public string Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        public int Grade
        {
            get => _grade;
            set => _grade = value;
        }

        public string Tags
        {
            get => _tags;
            set => _tags = value;
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public float Armor
        {
            get => _armor;
            set => _armor = value;
        }

        public float Power
        {
            get => _power;
            set => _power = value;
        }

        public float TimeBetweenAttacks
        {
            get => _timeBetweenAttacks;
            set => _timeBetweenAttacks = value;
        }

        public int Range
        {
            get => _range;
            set => _range = value;
        }

        public float CriticalDamageChance
        {
            get => _criticalDamageChance;
            set => _criticalDamageChance = value;
        }

        public float CriticalDamageMultiplier
        {
            get => _criticalDamageMultiplier;
            set => _criticalDamageMultiplier = value;
        }

        public float ChanceOfDodge
        {
            get => _chanceOfDodge;
            set => _chanceOfDodge = value;
        }

        public int Energy
        {
            get => _energy;
            set => _energy = value;
        }

        public int PowerOfHealing
        {
            get => _powerOfHealing;
            set => _powerOfHealing = value;
        }

        public float AdditionalParameter
        {
            get => _additionalParameter;
            set => _additionalParameter = value;
        }

        public PriorityConfig PriorityConfig
        {
            get => _priorityConfig;
            set => _priorityConfig = value;
        }

        public string Skill
        {
            get => _skill;
            set => _skill = value;
        }

        public int Level
        {
            get => _startLevel;
            set => _startLevel = value;
        }

        public void Perform(Character player)
        {
            player._uid = _uid;
            player._class = _class;
            player._prefab = _prefab;
            player._grade = _grade;
            player._tags = _tags;
            player._health = _health;
            player._armor = _armor;
            player._power = _power;
            player._timeBetweenAttacks = _timeBetweenAttacks;
            player._range = _range;
            player._criticalDamageChance = _criticalDamageChance;
            player._criticalDamageMultiplier = _criticalDamageMultiplier;
            player._chanceOfDodge = _chanceOfDodge;
            player._energy = _energy;
            player._powerOfHealing = _powerOfHealing;
            player._additionalParameter = _additionalParameter;
            player._skill = _skill;
            player._startLevel = _startLevel;
        }
    }
}