using System;
using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Shared.Extensions;
using Plugins.Ship.Sheets.InfoSheet;
using Units;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    public class CharacterInfoBuilder : InfoBuilder<Character>
    {
        private string _uid;
        private MinionClass _class;
        private string _prefab;
        private int _grade;
        private string _tags;
        private int _health;
        private float _armor;
        private float _power;
        private float _timeBetweenAttacks;
        private int _range;
        private float _criticalDamageChance;
        private float _criticalDamageMultiplier;
        private float _chanceOfDodge;
        private int _energy;
        private int _powerOfHealing;
        private float _additionalParameter;
        private string _skill;

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => _uid = s));
            queue.Enqueue(AddClass);
            queue.Enqueue((s => _prefab = s));
            queue.Enqueue((s => int.TryParse(s, out _grade)));
            queue.Enqueue((s => _tags = s));
            queue.Enqueue((s => int.TryParse(s, out _health)));
            queue.Enqueue((s => Parse.Float(s, out _armor, _uid)));
            queue.Enqueue((s => Parse.Float(s, out _power, _uid)));
            queue.Enqueue((s => Parse.Float(s, out _timeBetweenAttacks, _uid)));
            queue.Enqueue((s => int.TryParse(s, out _range)));
            queue.Enqueue((s => Parse.Float(s, out _criticalDamageChance, _uid)));
            queue.Enqueue((s => Parse.Float(s, out _criticalDamageMultiplier, _uid)));
            queue.Enqueue((s => Parse.Float(s, out _chanceOfDodge, _uid)));
            queue.Enqueue((s => int.TryParse(s, out _energy)));
            queue.Enqueue((s => int.TryParse(s, out _powerOfHealing)));
            queue.Enqueue((s => Parse.Float(s, out _additionalParameter, _uid)));
            queue.Enqueue((s => _skill = s));
        }

        private void AddClass(string value)
        {
            if (Enum.TryParse<MinionClass>(value, out var result))
            {
                _class = result;
            }
        }

        protected override IInfo<Character> GetInternal()
            => new Character()
            {
                Uid = _uid,
                Class = _class,
                Prefab = _prefab,
                Grade = _grade,
                Tags = _tags,
                Health = _health,
                Armor = _armor,
                Power = _power,
                TimeBetweenAttacks = _timeBetweenAttacks,
                Range = _range,
                CriticalDamageChance = _criticalDamageChance,
                CriticalDamageMultiplier = _criticalDamageMultiplier,
                ChanceOfDodge = _chanceOfDodge,
                Energy = _energy,
                PowerOfHealing = _powerOfHealing,
                AdditionalParameter = _additionalParameter,
                Skill = _skill,
            };
    }
    
    public class TutorialNodeBuilder : InfoBuilder<TutorialNode>
    {
        private string _uid;
        private string _startConditions;
        private string _actions;
        private string _endConditions;
        

        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => _uid = s));
            queue.Enqueue((s => _startConditions = s));
            queue.Enqueue((s => _actions = s));
            queue.Enqueue((s => _endConditions = s));
        }
        
        protected override IInfo<TutorialNode> GetInternal()
            => new TutorialNode()
            {
                Uid = _uid,
                StartConditions = _startConditions,
                Actions = _actions,
                EndConditions = _endConditions,
            };
    }

    [Serializable]
    public class TutorialNode : IInfo<TutorialNode>
    {
        [SerializeField] private string _uid;
        [SerializeField] private string _startConditions;
        [SerializeField] private string _actions;
        [SerializeField] private string _endConditions;

        public string Uid
        {
            get => _uid;
            set => _uid = value;
        }

        public string StartConditions
        {
            get => _startConditions;
            set => _startConditions = value;
        }

        public string Actions
        {
            get => _actions;
            set => _actions = value;
        }

        public string EndConditions
        {
            get => _endConditions;
            set => _endConditions = value;
        }

        public void Perform(TutorialNode info)
        {
            info._uid = _uid;
            info._startConditions = _startConditions;
            info._actions = _actions;
            info._endConditions = _endConditions;
        }
    }
}