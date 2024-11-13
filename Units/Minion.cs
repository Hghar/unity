using System;
using System.Collections;
using System.Collections.Generic;
using Animators;
using Battle;
using CompositeDirectorWithGeneratingComposites.CompositeDirector.CompositeGeneration;
using CustomInput;
using DG.Tweening;
using Fight;
using Fight.Attack;
using Fight.Damaging;
using Fight.Fractions;
using Fight.Healing.View;
using Fight.Targeting;
using Grids;
using Helpers.Position;
using Mights;
using Model.Commands;
using Model.Commands.Creation;
using Model.Commands.Parts;
using Model.Economy;
using Movement;
using Movement.Estimators;
using NaughtyAttributes;
using Parameters;
using Pathfinding;
using Realization.Configs;
using Realization.States.CharacterSheet;
using Realization.General;
using Realization.NewMovers;
using Realization.VisualEffects;
using TMPro;
using UnitSelling.Behaviours;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Zenject;
using CharacterStore = Parameters.CharacterStore;
using CharacterView = AssetStore.HeroEditor.Common.CharacterScripts.Character;
using Constants = Realization.Configs.Constants;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace Units
{
    [RequireComponent(typeof(IAstarAI))]
    public abstract class Minion<T> : MonoBehaviour, IMinion, IUnit where T : IUnitParameters
    {
        private const float MOVE_DURATION = 0.5f;
        
        [SerializeField] private Attacker _attacker;
        [SerializeField] private Damageable _damageable;
        [SerializeField] private Mortality _mortality;
        [SerializeField] private TargetDistanceEstimator _targetDistanceEstimator;
        [SerializeField] private HealingShower _healingShower;
        [SerializeField] private Flipper _flipper;
        [SerializeField] private Transform _characterParent;
        [SerializeField] private Might _might;
        [SerializeField] private MovementAnimator _movementAnimator;
        [SerializeField] private AttackAnimator _attackAnimator;
        [SerializeField] private DeathAnimator _deathAnimator;
        [SerializeField] private EnergyStorage _energyStorage;
        [SerializeField] private Draggable _draggable;
        [SerializeField] private TMP_Text _testTarget;
        [SerializeField] private Level _level;
        [SerializeField] private SellingHandler _sellingHandler;
        
        private IAstarAI _astar;
        private CharacterView _character;
        private Vector2 _healSpread;
        private Vector2Int _position;
        private IGrid<IMinion> _grid;
        private bool _dragging;
        private bool _died;
        private PriorityConfig _priorityConfig;
        private Vector3 _oldPosition;
        private bool _stunned;
        private CommandInvoker _commandInvoker;
        private float _damageReflection;
        private int _grade;
        private IVisualEffectService _visualEffectService;

        public SellingHandler SellingHandler => _sellingHandler;
        public Flipper Flipper => _flipper;
        private bool _isMinion = true;
        private bool _isBoss = false;

        public Level Level => _level;
        public Might Might => _might;
        public T ClassParameters { get; protected set; }
        public bool IsDragging => _dragging;
        public int Shields => _damageable.ShieldSumm;
        public bool Silenced => _commandInvoker.Working == false;
        public bool PassiveSkills => _commandInvoker.OnlyPassives;
        public IUnitParameters Parameters => ClassParameters;
        public CharacterStore CharacterStore { get; protected set; }
        public GameObject GameObject 
        {
            get
            {
                try
                {
                    if (gameObject)
                        return gameObject;
                }
                catch (Exception _)
                {
                    return null;
                }

                return null;
            }
        }
        public MinionClass Class { get; private set; }

        public ClassParent ParentId
        {
            get
            {
                if (Class == MinionClass.Gladiator || Class == MinionClass.Templar) return ClassParent.Warrior;
                if (Class == MinionClass.Ranger || Class == MinionClass.Assassin) return ClassParent.Scout;
                if (Class == MinionClass.Spiritmaster || Class == MinionClass.Sorcerer) return ClassParent.Mage;
                if (Class == MinionClass.Chanter || Class == MinionClass.Cleric) return ClassParent.Priest;
                return ClassParent.None;
            }
        }

        public int Grade => _grade;
        public string Name => name;
        public Vector2Int Position { 
            get => _position;
            set
            {
                _position = value;
                UpdateWorldPosition(MoveType.Translate);
            } 
        }
        public Vector2 WorldPosition => _damageable.WorldPosition;

        public bool IsDamage => true;
        public int Priority => _damageable.Priority;
        public float Aggression { get; private set; }
        public float BonusAggression { get; private set; }
        public Fraction Fraction => _damageable.Fraction;
        public bool Initialized { get; private set; }
        public IMinion Target => _targetDistanceEstimator.TargetFinder.MinionTarget;
        public bool IsMoving => (_oldPosition != transform.position);
        public string Uid { get; private set; }
        public bool IsDestroying => _damageable.IsDestroying;
        public float PersonalMight => _might.PersonalMight;
        public PriorityConfig PriorityConfig => _priorityConfig;
        public float EnergyValue => _energyStorage.EnergyValue;
        public float EnergyMaxValue => _energyStorage.MaxValue;
        public bool IsMinion => _isMinion;

        public bool IsBoss => _isBoss;

        public event Action<IDestroyablePoint> Destroying;
        public event Action<IMinion> Died;
        public event Action Dragged;
        public event Action Disposed;

        private Character _configCharacter;
        private CurrencyValuePair _selling;
        private int _stuns;
        private IBattleStartPublisher _battleStartPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;

        private void Awake()
        {
            _astar = GetComponent<AILerp>();
            _testTarget = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            UpdateWorldPosition(MoveType.Translate);
        }

        [Inject]
        private void Construct(IBattleStartPublisher battleStartPublisher, IBattleFinishPublisher finishPublisher,
            IVisualEffectService visualEffectService)
        {
            _visualEffectService = visualEffectService;
            _battleStartPublisher = battleStartPublisher;
            _battleStartPublisher.BattleStarted += OnBattleStarter;
            _battleFinishPublisher = finishPublisher;
            _battleFinishPublisher.BattleFinished += OnBattleFinished;
        }

        private void OnBattleStarter()
        {
            HardUnsilence();
            _commandInvoker.PerformPassives();
            GetComponentInChildren<FightTargetFinder>().FindTargets();
            Parameters.Health.Immortality = false;
        }

        private void OnBattleFinished()
        {
            HardSilence();
            if (Fraction == Fraction.Minions)
                Parameters.Health.Immortality = true;
            _commandInvoker.UndoPassives();
            _energyStorage.Empty();
            _damageable.RemoveAddShields();
        }

        public void Initialize(Character config, CurrencyValuePair selling, Constants constants, IGrid<IMinion> grid,
            PriorityConfig priorityConfig, CommandFacade commandFacade, Skill[] skills)
        {
            if (config.Uid.Contains("Summoned"))
            {
                _isMinion = false;

                Destroy(this.transform.Find("SellableBehaviour").gameObject);
            }

            if (config.Uid.Contains("Boss"))
                _isBoss = true;

            _grade = config.Grade;
            Aggression = priorityConfig.GoalAggression;
            
            if(_level == null)
            {
                _level = this.GetComponentInChildren<Level>();
            }

            _configCharacter = config;
            _selling = selling;

            _priorityConfig = priorityConfig;

            ClassParameters = GetParametersInternal(config, selling, _level); 

            Class = config.Class;
            Uid = config.Uid;
            _grid = grid;
            _grade = config.Grade;

            _mortality.SetConfig(Parameters);
            
            _attacker.SetConfig(this, Parameters, constants);
            _targetDistanceEstimator.SetConfig(Parameters);
            _healingShower.SetConfig(this, Parameters);
            Debug.Log(name);
            _energyStorage.SetConfig(Parameters, (Parameters.Health as Health));
            
            var fraction = config.Tags.Contains("enemy") ? Fraction.Enemies : Fraction.Minions;

            if (fraction == Fraction.Enemies)
            {
                _draggable.enabled = false;
            }
            _damageable.SetConfig(this, Parameters, fraction);
            _character = CharacterHelper.GetVisual(CharacterParent, config.Prefab);
            _movementAnimator.SetCharacter(_character);
            _attackAnimator.SetCharacter(_character);
            _deathAnimator.SetCharacter(_character);
            _might.SetConfig(config, constants, (Parameters.Health as Health));

            _healSpread = constants.BattleHealingSpread;

            _mortality.Dying += Die;
            _damageable.Destroying += UnitDestroing;
            _draggable.DraggingBegun += () =>
            {
                (_astar as MonoBehaviour).enabled = false;
                _dragging = true;
            };
            _draggable.DraggingFinished += () =>
            {
                Dragged?.Invoke();
                _dragging = false;
            };
            
            _commandInvoker = new CommandInvoker(skills, commandFacade, _energyStorage, this);
            // HardSilence();
            HardUnsilence();

            _level.Inicializade(this, config.Level);

            Initialized = true;
        }

        public void UpdateInicialize()
        {
            _attacker.UpdateConfig(Parameters);
            _mortality.UpdateConfig(Parameters);
            _targetDistanceEstimator.UpdateConfig(Parameters);
            _healingShower.UpdateConfig(Parameters);
            _damageable.UpdateConfig(Parameters);
            _might.UpdateConfig(Parameters);
        }

        public void UpdateListMinion(IMinion[] minions)
        {
            _attacker.minions = minions;
        }

        private void OnDestroy()
        {
            Dragged = null;
            _grid.Unbind(this);
            _commandInvoker.Dispose();
            _battleStartPublisher.BattleStarted -= OnBattleStarter;
            _battleFinishPublisher.BattleFinished -= OnBattleFinished;
            // if(_died == false)
            //     Died?.Invoke(this);
            Died = null;
            Dispose();
        }

        private void Update()
        {
            if (_oldPosition != transform.position)
            {
                _movementAnimator.Run();
            }
            else
            {
                _movementAnimator.Ready();
            }

            _oldPosition = transform.position;
            try
            {
                _testTarget.text = _targetDistanceEstimator.TargetFinder?.MinionTarget?.Name;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        private void UnitDestroing(IDestroyablePoint obj)
        {
            Destroying?.Invoke(this);
            _damageable.Destroying -= UnitDestroing;
        }

        public void DiedMinion(IMinion minion)
        {
            if (_attacker.minions != null)
            {
                if (minion.Fraction != Fraction.Minions && minion.IsMinion && IsMinion)
                {
                    _level.AddPoints(minion, _attacker.minions);
                }
            }
        }

        private void Die()
        {
            _battleStartPublisher.BattleStarted -= OnBattleStarter;
            _battleFinishPublisher.BattleFinished -= OnBattleFinished;
            _grid.Unbind(this);
            Died?.Invoke(this);
            _died = true;
        }

        public int Heal(int value)
        {
            var spread = Random.Range(_healSpread.x, _healSpread.y);

            //TODO: healing bonuses
            float bonuses = 0;
            int healing = (int)((1 + bonuses) * spread * value);
            Debug.Log($"{GameObject.name} healed: {healing}");
            Parameters.Health.Increase(healing);
            _visualEffectService.Create(VisualEffectType.Heal, this);
            return healing;
        }

        public void Damage(IDamage damage)
        {
            _damageable.TakeDamage(damage, out _);
        }
        
        public void Damage(IDamage damage, IMinion caster)
        {
            if (_damageReflection != 0)
            {
                var oldHealth = Parameters.Health.Value;
                Damage(damage);
                caster.Damage(new Damage((oldHealth - Parameters.Health.Value) * (_damageReflection * 0.01f)));
                return;
            }
            
            Damage(damage);
        }

        public void IncreaseView(float scale)
        {
            _flipper.SetDefaultSize(new Vector3(scale, scale, scale));
        }

        public void UpdateWorldPosition(MoveType type)
        {
            if((_astar as MonoBehaviour) == null || _died)
                return;
            
            (_astar as MonoBehaviour).enabled = false;
            transform.DOKill();
            
            switch (type)
            {
                case MoveType.Instantly:
                    transform.position = _grid.WorldCoordinates(this);
                    break;
                case MoveType.Translate:
                    transform.DOMove(_grid.WorldCoordinates(this), MOVE_DURATION);
                    break;
                case MoveType.AStar:
                    (_astar as MonoBehaviour).enabled = true;
                    _astar.destination = _grid.WorldCoordinates(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public bool NeedToMove()
        {
            return _attacker.IsCloseToTarget == false && _stunned == false;
        }

        public void DisableAi()
        {
            (_astar as MonoBehaviour).enabled = false;
        }
        
        public int PercentHeal(int percents)
        {
            _visualEffectService.Create(VisualEffectType.Heal, this);
            int value;
            float percentMax = percents * 0.01f;
            value = (int) (Parameters.Health.MaxValue * percentMax);
            Parameters.Health.Increase(value);
            return value;
        }

        public void Fight()
        {
            GetComponentInChildren<BattleStartHandler>().StartBattle();
            GetComponentInChildren<FightTargetFinder>().StartBattle();
            HardUnsilence();
            _commandInvoker.PerformPassives();
            _attacker.StartAttacking();
        }

        public int PercentDamage(int percents, string healthTarget)
        {
            _visualEffectService.Create(VisualEffectType.Damage, this);
            int value;
            switch (healthTarget)
            {
                case "Current":
                    float percentCurrent = percents * 0.01f;
                    value = (int) (Parameters.Health.Value * percentCurrent);
                    Damage(new Damage(value));
                    return value;
                case "Max":
                    float percentMax = percents * 0.01f;
                    value = (int) (Parameters.Health.MaxValue * percentMax);
                    Damage(new Damage(value));
                    return value;
                default:
                    throw new Exception($"Health target {healthTarget} is not valid");
            }
        }

        public void AddShield(int shieldValue, CommandClass commandClass)
        {
            shieldValue = Math.Min(Parameters.Health.MaxValue - _damageable.ShieldSumm, shieldValue);
            if(shieldValue <= 0)
                return;
            
            if (_damageable.HasShield(commandClass))
            {
                _damageable.UpdateShield(commandClass, shieldValue);
                return;
            }
            
            _damageable.TryAddShield(shieldValue, commandClass);
        }

        public void RemoveShield(CommandClass commandClass)
        {
            _damageable.RemoveShield(commandClass);
            // _visualEffectService.EndEffectWithDuration(VisualEffectType.Shield, this);
        }

        public void AddEnergy(float value, bool withEffect)
        {
            if(withEffect)
                _visualEffectService.Create(VisualEffectType.Energy, this);
            _energyStorage.EnergyValue += value;
        }

        public void AddLevelPoints(float points)
        {
            if (this.Fraction != Fraction.Enemies)
            {
                _level.CheatAddPoint(points);
            }
        }

        public string ReturnLevelPoint => _level.LevelIntoText();

        public Transform CharacterParent => _characterParent;

        protected abstract T GetParametersInternal(Character config, CurrencyValuePair selling, Level level);
        public void Dispose()
        {
            _commandInvoker.Dispose();
            Disposed?.Invoke();
        }

        public int IncreaseHealth(int value)
        {
            var oldValue = Parameters.Health.MaxValue;
            Parameters.Health.IncreaseMax(value);
            return Parameters.Health.MaxValue - oldValue;
        }

        public int DecreaseHealth(int value)
        {
            var oldValue = Parameters.Health.MaxValue;
            Parameters.Health.DecreaseMax(value);
            return oldValue - Parameters.Health.MaxValue;
        }

        public int IncreaseArmor(int value)
        {
            var oldValue = Parameters.Armor.Value;
            Parameters.Armor.Increase(value);
            return (int)(Parameters.Armor.Value - oldValue);
        }

        public int DecreaseArmor(int value)
        {
            var oldValue = Parameters.Armor.Value;
            Parameters.Armor.Decrease(value);
            return (int)(oldValue - Parameters.Armor.Value);
        }

        public int IncreasePower(int value)
        {
            var oldValue = Parameters.Damage.Value;
            Parameters.Damage.Increase(value);
            return (int)(Parameters.Damage.Value - oldValue);
        }

        public int DecreasePower(int value)
        { 
            var oldValue = Parameters.Damage.Value;
            Parameters.Damage.Decrease(value);
            return (int)(oldValue - Parameters.Damage.Value);
        }

        public float IncreaseTimeBetweenAttacks(float value)
        {
            var oldValue = Parameters.Cooldown.Value;
            Parameters.Cooldown.Decrease(value);
            return (oldValue - Parameters.Cooldown.Value);
        }

        public float DecreaseTimeBetweenAttacks(float value)
        {
            var oldValue = Parameters.Cooldown.Value;
            Parameters.Cooldown.Increase(value);
            return (Parameters.Cooldown.Value-oldValue);
        }

        public float IncreaseCriticalChance(float value)
        {
            var oldValue = Parameters.ChanceOfCriticalDamage.Value;
            Parameters.ChanceOfCriticalDamage.Increase(value);
            return (Parameters.ChanceOfCriticalDamage.Value-oldValue);
        }

        public float DecreaseCriticalChance(float value)
        {
            var oldValue = Parameters.ChanceOfCriticalDamage.Value;
            Parameters.ChanceOfCriticalDamage.Decrease(value);
            return (oldValue - Parameters.ChanceOfCriticalDamage.Value);
        }

        public float IncreaseCriticalMultiplier(float value)
        {
            var oldValue = Parameters.CriticalDamageMultiplier.Value;
            Parameters.CriticalDamageMultiplier.Increase(value);
            return (Parameters.CriticalDamageMultiplier.Value-oldValue);
        }

        public float DecreaseCriticalMultiplier(float value)
        {
            var oldValue = Parameters.CriticalDamageMultiplier.Value;
            Parameters.CriticalDamageMultiplier.Decrease(value);
            return (oldValue - Parameters.CriticalDamageMultiplier.Value);
        }

        public float IncreaseDodgeChance(float value)
        {
            var oldValue = Parameters.Agility.Value;
            Parameters.Agility.Increase(value);
            return (Parameters.Agility.Value-oldValue);
        }

        public float DecreaseDodgeChance(float value)
        {
            var oldValue = Parameters.Agility.Value;
            Parameters.Agility.Decrease(value);
            return (oldValue - Parameters.Agility.Value);
        }

        public Result FixedDamage(int value)
        {
            _visualEffectService.Create(VisualEffectType.Damage, this);
            Damage(new Damage(value));
            return Result.Ok;
        }

        public Result FixedHeal(int value)
        {
            _visualEffectService.Create(VisualEffectType.Heal, this);
            Parameters.Health.Increase(value);
            return Result.Ok;
        }

        public Result Perform(IMinionCommand commandDamage)
        {
            if (commandDamage == null)
                return  Result.Ok;
            
            commandDamage.Perform(this);
            return Result.Ok;
        }

        public Result Unperform(IMinionCommand commandDamage)
        {
            if (commandDamage == null)
                return  Result.Ok;
            
            commandDamage.Undo(this);
            return Result.Ok;
        }

        public float IncreaseAggression(float value)
        {
            BonusAggression += value;
            return value;
        }

        public float DecreaseAggression(float value)
        {
            var old = BonusAggression;
            BonusAggression -= value;
            BonusAggression = Mathf.Max(0, BonusAggression);
            return old-BonusAggression;
        }

        public void UpdateTarget(IMinion defectivMinion)
        {
            throw new NotImplementedException();
        }

        public Result RecalculatePriorities()
        {
            _attacker.FindNewTarget();
            return Result.Ok;
        }

        public Result DecreaseFixedEnergy(int value)
        {
            _visualEffectService.Create(VisualEffectType.EnergyDown, this);
            _energyStorage.EnergyValue -= value;
            return Result.Ok;
        }

        public Result IncreaseFixedEnergy(int value)
        {
            _visualEffectService.Create(VisualEffectType.Energy, this);
            _energyStorage.EnergyValue = (_energyStorage.EnergyValue + value);
            return Result.Ok;
        }

        public Result IncreasePercentEnergy(int value)
        {
            _visualEffectService.Create(VisualEffectType.Energy, this);
            _energyStorage.EnergyValue += _energyStorage.MaxValue * (value * 0.01f);
            return Result.Ok;
        }

        public Result DecreasePercentEnergy(int value, string energyTarget)
        {
            _visualEffectService.Create(VisualEffectType.EnergyDown, this);
            var calculatedValue = 0f;
            if(energyTarget == "Max")
                calculatedValue =  Parameters.Energy.MaxValue * (value * 0.01f);
            else
                calculatedValue = Parameters.Energy.Value * (value * 0.01f);
            _energyStorage.EnergyValue -= calculatedValue;
            return Result.Ok;
        }

        public void FixedLifesteal(int value, IMinion caster)
        {
            // Debug.LogError($"perform passive {Name}");
            _visualEffectService.Create(VisualEffectType.Lifesteal, this);
            var oldHealth = Parameters.Health.Value;
            Damage(new Damage(value));
            var newHealth = oldHealth - Parameters.Health.Value;
            caster.Parameters.Health.Increase(newHealth);
        }

        public void PercentLifesteal(int value, string healthTarget, IMinion caster)
        {
            _visualEffectService.Create(VisualEffectType.Lifesteal, this);
               
            var calculatedValue = 0f;
            if(healthTarget == "Max")
                calculatedValue =  Parameters.Health.MaxValue * (value * 0.01f);
            else
                calculatedValue = Parameters.Health.Value * (value * 0.01f);
            
            var oldHealth = Parameters.Health.Value;
            Debug.Log($"Percent lifesetal {calculatedValue}");
            Damage(new Damage(calculatedValue));
            var newHealth = oldHealth - Parameters.Health.Value;
            caster.Parameters.Health.Increase(newHealth);
        }

        public Result Stun()
        {
            _visualEffectService.Create(VisualEffectType.Stun, this);
            _visualEffectService.CreateEffectWithDuration(VisualEffectType.Stunning, this);
            _attacker.Disable();
            _stunned = true;
            _stuns++;
            return Result.Ok;
        }

        public Result Unstun()
        {
            _stuns--;
            if (_stuns > 0)
                return Result.Ok;
            
            _visualEffectService.EndEffectWithDuration(VisualEffectType.Stunning, this);
            _attacker.Activate();
            _stunned = false;
            return Result.Ok;
        }

        public Result Silence()
        {
            _visualEffectService.Create(VisualEffectType.Silence, this);
            _visualEffectService.CreateEffectWithDuration(VisualEffectType.Silencing, this);
            _commandInvoker.Disable();
            return Result.Ok;
        }

        public Result Unsilence()
        {
            _visualEffectService.EndEffectWithDuration(VisualEffectType.Silencing, this);
            _commandInvoker.Activate();
            _commandInvoker.TryPerform();
            return Result.Ok;
        }

        public Result Immortality()
        {
            _visualEffectService.CreateEffectWithDuration(VisualEffectType.Immortality, this);
            Parameters.Health.Lock();
            return Result.Ok;
        }

        public Result Unimmortal()
        {
            _visualEffectService.EndEffectWithDuration(VisualEffectType.Immortality, this);
            Parameters.Health.Unlock();
            return Result.Ok;
        }

        public Result DamageReflection(int percents)
        {
            _visualEffectService.CreateEffectWithDuration(VisualEffectType.Reflection, this);
            _damageReflection += percents;
            return Result.Ok;
        }
        
        public Result UndoDamageReflection(int percents)
        {
            _visualEffectService.EndEffectWithDuration(VisualEffectType.Reflection, this);
            _damageReflection -= percents;
            return Result.Ok;
        }
        
        public void HardSilence()
        {
            // _visualEffectService.Create(VisualEffectType.Silence, this);
            _commandInvoker.Disable();
            _commandInvoker.DisablePassives();
        }
        
        public void HardUnsilence()
        {
            // _visualEffectService.Create(VisualEffectType.Silence, this);
            _commandInvoker.Activate();
            _commandInvoker.ActivatePassives();
        }

        public void LevelUp(Constants constants)
        {
            float newHealth = ClassParameters.Health.MaxValue;
            float newPower = ClassParameters.Damage.Value;
            float newPower_of_healing = _configCharacter.PowerOfHealing;

            switch (_configCharacter.Grade)
            {
                case 1:
                    newHealth += (newHealth) * constants.LvlUpBoosterGrade1.x;
                    newPower += (newPower) * constants.LvlUpBoosterGrade1.y;
                    newPower_of_healing += (newPower_of_healing) * constants.LvlUpBoosterGrade1.z;
                    break;
                case 2:
                    newHealth += (newHealth) * constants.LvlUpBoosterGrade2.x;
                    newPower += (newPower) * constants.LvlUpBoosterGrade2.y;
                    newPower_of_healing += (newPower_of_healing) * constants.LvlUpBoosterGrade2.z;
                    break;
                case 3:
                    newHealth += (newHealth) * constants.LvlUpBoosterGrade3.x;
                    newPower += (newPower) * constants.LvlUpBoosterGrade3.y;
                    newPower_of_healing += (newPower_of_healing) * constants.LvlUpBoosterGrade3.z;
                    break;
                case 4:
                    newHealth += (newHealth) * constants.LvlUpBoosterGrade4.x;
                    newPower += (newPower) * constants.LvlUpBoosterGrade4.y;
                    newPower_of_healing += (newPower_of_healing) * constants.LvlUpBoosterGrade4.z;
                    break;
                case 5:
                    newHealth += (newHealth) * constants.LvlUpBoosterGrade5.x;
                    newPower += (newPower) * constants.LvlUpBoosterGrade5.y;
                    newPower_of_healing += (newPower_of_healing) * constants.LvlUpBoosterGrade5.z;
                    break;
            }

            ClassParameters = UpParametersInternal(ClassParameters ,_configCharacter,
                MathF.Round(newHealth),
                MathF.Round(newPower),
                MathF.Round(newPower_of_healing),
                _selling, _level);

            UpdateInicialize();
        }

        protected abstract T UpParametersInternal(T unitParameters ,Character config, float health, float power, float power_of_healing, CurrencyValuePair selling, Level level);
    }
}