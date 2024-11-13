using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Shared.Extensions;
using Parameters;
using Plugins.Ship.Sheets.InfoSheet;
using Units;

namespace Realization.States.CharacterSheet
{
    public class GeneralLevelingUpBuilder : InfoBuilder<GeneralLevelingUpConfig>
    {
        private const string Separator = ";";
        private int _level;
        private int _goldCost;

        private int _health;
        private int _armor;
        private float _power;
        private float _criticalDamageChance;
        private float _criticalDamageMultiplier;
        private float _dodgeChance;
        private float _healPower;


        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => { int.TryParse(s, out _level); }));
            queue.Enqueue((s => { int.TryParse(s, out _goldCost); }));
            queue.Enqueue(s =>
            {
                _health = 0;
                _armor = 0;
                _power = 0;
                _criticalDamageChance = 0;
                _criticalDamageMultiplier = 0;
                _dodgeChance = 0;
                _healPower = 0;
                var strings = s.Split(Separator);
                foreach (var text in strings)
                {
                    if (text.Contains(ParseHelper.Keys.ArmorKey)) 
                        ParseHelper.ParseTo(out _armor, text);
                    if (text.Contains(ParseHelper.Keys.HealthKey))
                        ParseHelper.ParseTo(out _health, text);
                    if (text.Contains(ParseHelper.Keys.PowerKey)) 
                        ParseHelper.ParseTo(out _power, text);
                    if (text.Contains(ParseHelper.Keys.Critical_damage_chance_Key))
                        ParseHelper.ParseTo(out _criticalDamageChance, text);
                    if (text.Contains(ParseHelper.Keys.Chance_of_dodge_Key))
                        ParseHelper.ParseTo(out _dodgeChance, text);
                    if (text.Contains(ParseHelper.Keys.Power_of_healing_Key))
                        ParseHelper.ParseTo(out _healPower, text);
                    if (text.Contains(ParseHelper.Keys.Critical_damage_multiplier_Key))
                        ParseHelper.ParseTo(out _criticalDamageMultiplier, text);
                }
            });
        }

        protected override IInfo<GeneralLevelingUpConfig> GetInternal()
            => new GeneralLevelingUpConfig()
            {
                    Level = _level,
                    GoldCost = _goldCost,
                    Health = _health,
                    Armor = _armor,
                    Power = _power,
                    CriticalDamageChance = _criticalDamageChance,
                    CriticalDamageMultiplier = _criticalDamageMultiplier,
                    DodgeChance = _dodgeChance,
                    HealPower = _healPower,
            };
    }

    public class ClassLevelingUpBuilder : InfoBuilder<LevelingUpConfig>
    {
        private int _level;
        private int _tokenCost;
        private int _goldCost;
        private string _mages;
        private string _warriors;
        private string _priests;
        private string _scouts;


        protected override void SetQueue(Queue<Action<string>> queue)
        {
            queue.Enqueue((s => { int.TryParse(s, out _level); }));
            queue.Enqueue((s => { int.TryParse(s, out _tokenCost); }));
            queue.Enqueue((s => { int.TryParse(s, out _goldCost); }));
            queue.Enqueue((s => { _warriors = s; }));
            queue.Enqueue((s => { _priests = s; }));
            queue.Enqueue((s => { _mages = s; }));
            queue.Enqueue((s => { _scouts = s; }));
        }

        protected override IInfo<LevelingUpConfig> GetInternal()
        {
            var config = new LevelingUpConfig()
            {
                    Stats = new StatsConfig(),
                    Level = _level,
                    GoldCost = _goldCost,
                    TokenCost = _tokenCost,
            };
            config.Stats.List.Add(CreateData(_mages, ClassParent.Mage));
            config.Stats.List.Add(CreateData(_priests, ClassParent.Priest));
            config.Stats.List.Add(CreateData(_warriors, ClassParent.Warrior));
            config.Stats.List.Add(CreateData(_scouts, ClassParent.Scout));
            return config;
        }

        private StatsData CreateData(string data, ClassParent id)
        {
            StatsData statsData = new StatsData(id);

            var strings = data.Split(";");
            foreach (var text in strings)
            {
                if (text.Contains(ParseHelper.Keys.HealthKey))
                    ParseHelper.ParseTo(out statsData.Health, text);
                if (text.Contains(ParseHelper.Keys.ArmorKey))
                    ParseHelper.ParseTo(out statsData.Armor, text);
                if (text.Contains(ParseHelper.Keys.PowerKey))
                    ParseHelper.ParseTo(out statsData.Power, text);
            }

            return statsData;
        }
    }

    public static class ParseHelper
    {
        private const char PointChar = '.';
        public  class Keys
        {
            public static string ArmorKey = "Armor";
            public static string HealthKey = "Health";
            public static string PowerKey = "Power";
            public static string Critical_damage_chance_Key = "Critical_damage_chance";
            public static string Chance_of_dodge_Key = "Chance_of_dodge";
            public static string Power_of_healing_Key = "Force_of_healing";
            public static string Critical_damage_multiplier_Key = "Critical_damage_multiplier";
        }
        public static void ParseTo(out float armor, string text)
        {
            string numeric = new String(text.Where(SelectFloatNumber()).ToArray());
            Parse.Float(numeric, out armor);
        }

        public static void ParseTo(out int armor, string text)
        {
            string numeric = new String(text.Where(SelectFloatNumber()).ToArray());
            int.TryParse(numeric, out armor);
        }

        private static Func<char, bool> SelectFloatNumber()
        {
            return x => x == PointChar || Char.IsDigit(x);
        }
    }
}