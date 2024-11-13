using System;
using AStar;
using System.Collections.Generic;
using Fight.Damaging;
using Fight.Fractions;
using Fight.Damaging;
using Grids;
using Helpers.Position;
using Mights;
using Model.Commands;
using Model.Economy;
using Parameters;
using Point;
using Realization.Configs;
using Realization.States.CharacterSheet;
using UnityEngine;
using Mights;
using Model.Commands.Parts;
using Movement;
using UnitSelling.Behaviours;
using Constants = Realization.Configs.Constants;

namespace Units
{
    public interface IMinion : IDestroyablePoint, IGridObject, IAffectable
    {
        event Action<IMinion> Died;
        event Action Dragged;

        SellingHandler SellingHandler { get; }
        Flipper Flipper { get; }
        IUnitParameters Parameters { get; }
        GameObject GameObject { get; }
        MinionClass Class { get; }
        ClassParent ParentId { get; }
        public bool IsDamage { get; }
        public int Priority { get; }
        float Aggression { get; }
        public PriorityConfig PriorityConfig { get; }
        public Fraction Fraction { get; }
        public int Grade { get; }
        bool Initialized { get; }
        IMinion Target { get; }
        bool IsMoving { get; }
        string Uid { get; }

        Level Level { get; }
        Might Might { get; }
        float EnergyValue { get; }
        float EnergyMaxValue { get; }
        bool IsDragging { get; }
        int Shields { get; }
        bool Silenced { get; }
        bool PassiveSkills { get; }

        bool IsMinion { get; }

        bool IsBoss { get; }

        void Initialize(Character config, CurrencyValuePair selling, Constants constants, IGrid<IMinion> grid,
            PriorityConfig priorityConfig, CommandFacade commandFacade, Skill[] skills);
        void UpdateInicialize();
        int Heal(int value);
        void Damage(IDamage damage);
        void Damage(IDamage damage, IMinion caster);
        void IncreaseView(float scale);
        void UpdateWorldPosition(MoveType type);
        bool NeedToMove();
        void DisableAi();
        int PercentDamage(int percents, string healthTarget);
        void AddShield(int shieldValue, CommandClass commandClass);
        void RemoveShield(CommandClass commandClass);
        void AddEnergy(float value, bool withEffect = true);
        void HardSilence();
        void HardUnsilence();
        
        void DiedMinion(IMinion minion);
        void UpdateListMinion(IMinion[] minions);
        void LevelUp(Constants constants);

        void AddLevelPoints(float points);
        string ReturnLevelPoint { get; }
        float BonusAggression { get; }
        Transform CharacterParent { get; }


        int IncreaseHealth(int value);
        int DecreaseHealth(int value);
        int IncreasePower(int value);
        int DecreasePower(int value);
        float IncreaseTimeBetweenAttacks(float value);
        float DecreaseTimeBetweenAttacks(float value);
        float IncreaseCriticalMultiplier(float value);
        float DecreaseCriticalMultiplier(float value);
        int IncreaseArmor(int value);
        int DecreaseArmor(int value);
        float IncreaseCriticalChance(float different);
        float DecreaseCriticalChance(float value);
        float IncreaseDodgeChance(float value);
        float DecreaseDodgeChance(float value);
        int PercentHeal(int percents);
        void Fight();
        void FixedLifesteal(int value, IMinion caster);
        void PercentLifesteal(int value, string parameter, IMinion caster);
        float IncreaseAggression(float value);
        float DecreaseAggression(float value);
    }
}