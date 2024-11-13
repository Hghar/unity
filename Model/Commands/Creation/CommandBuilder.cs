using System;
using System.Linq;
using CompositeDirectorWithGeneratingComposites.CompositeDirector.CompositeGeneration;
using Fight.Fractions;
using Infrastructure.Shared.Extensions;
using Model.Commands.Helpers;
using Model.Commands.Parts;
using Model.Commands.Types;
using Realization.States.CharacterSheet;
using Realization.VisualEffects;
using Units;

namespace Model.Commands.Creation
{
    public class CommandBuilder
    {
        private readonly IAffectable _composite;
        private readonly CommandWorker _worker;
        private readonly IVisualEffectService _effectService;
        private IMinionPool _minionPool;
        private MinionFactory _minionFactory;
        private Character[] _characters;

        private CommandParameters _parameters = new();

        public CommandBuilder(IAffectable composite, CommandWorker worker, IVisualEffectService effectService)
        {
            _effectService = effectService;
            _worker = worker;
            _composite = composite;
        }

        public void Initialize(IMinionPool minionPool, MinionFactory minionFactory, Character[] characters)
        {
            _characters = characters;
            _minionFactory = minionFactory;
            _minionPool = minionPool;
        }

        private void SetType(CommandType type)
        {
            _parameters.Type = type;
        }
        
        private void SetAction(string action, CommandClass commandClass, params object[] parameters)
        {
            var actionReader = new ActionReader(_composite, _minionFactory, _characters, _effectService);
            var pair = actionReader.Read(action, commandClass, parameters);
            _parameters.Name = action;
            _parameters.Action = pair.Key;
            _parameters.Canceling = pair.Value;
        }
        
        private void SetTarget(TargetType type, params object[] parameters)
        {
            _parameters.Count = 0;
            IMinion caster;
            
            _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions;
            switch (type)
            {
                case TargetType.Self:
                    caster = (IMinion)parameters[0];
                    _parameters.TargetScope = (minion) => minion == caster;
                    break;
                case TargetType.AllAllies:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions;
                    break;
                case TargetType.RandomAlly:
                    _parameters.Count = (int)parameters[0];
                    var minions = 
                        _minionPool.Minions.Where((minion) => minion.Fraction == Fraction.Minions)
                            .ToList()
                            .Shuffle()
                            .Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => minions.Contains(minion);
                    break;
                case TargetType.AllyGladiators:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Gladiator;
                    break;
                case TargetType.AllyTemplars:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions &&  minion.Class == MinionClass.Templar;
                    break;
                case TargetType.AllyRangers:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Ranger;
                    break;
                case TargetType.AllyAssassins:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Assassin;
                    break;
                case TargetType.AllySpiritmasters:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Spiritmaster;
                    break;
                case TargetType.AllySorcerers:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Sorcerer;
                    break;
                case TargetType.AllyClerics:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Cleric;
                    break;
                case TargetType.AllyChanters:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Minions && minion.Class == MinionClass.Chanter;
                    break;
                case TargetType.NearestAlly:
                    _parameters.Count = (int)parameters[0];
                    caster = (IMinion)parameters[1];
                    var nearest =
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Minions)
                            .OrderBy((minion => Distance(minion, caster)));
                    
                    int minDistance = 0;
                    if(nearest.Any())
                        minDistance = Distance(nearest.ToList()[0], caster);
                    
                     var nearest1 = 
                         nearest
                             .Where((minion => Distance(minion, caster) == minDistance))
                             .ToList()
                             .Shuffle()
                             .Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => nearest1.Contains(minion);
                    break;
                case TargetType.MostWoundedAlly:
                    _parameters.Count = (int)parameters[0];
                    var wounded =
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Minions)
                            .OrderBy((minion =>
                                (float) minion.Parameters.Health.Value / minion.Parameters.Health.MaxValue));

                    float minHealth = 0;
                    if (wounded.Any())
                        minHealth = (float) wounded.ToList()[0].Parameters.Health.Value /
                                    wounded.ToList()[0].Parameters.Health.MaxValue;
                    
                    var minHealth1 = 
                        wounded
                            .Where((minion => Math.Abs((float) minion.Parameters.Health.Value / minion.Parameters.Health.MaxValue - minHealth) < 0.01f))
                            .ToList()
                            .Shuffle()
                            .Take(_parameters.Count);
                    
                    _parameters.TargetScope = (minion) => minHealth1.Contains(minion);
                    break;
                case TargetType.MostHealthiestAlly:
                    _parameters.Count = (int)parameters[0];
                    var healthiest =
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Minions)
                            .OrderByDescending((minion =>
                                (float) minion.Parameters.Health.Value / minion.Parameters.Health.MaxValue));
                    
                    float maxHealth = 0;
                    if (healthiest.Any())
                        maxHealth = (float) healthiest.ToList()[0].Parameters.Health.Value /
                                    healthiest.ToList()[0].Parameters.Health.MaxValue;
                    
                    var maxHealth1 = 
                        healthiest
                            .Where((minion => Math.Abs((float) minion.Parameters.Health.Value / minion.Parameters.Health.MaxValue - maxHealth) < 0.01f))
                            .ToList()
                            .Shuffle()
                            .Take(_parameters.Count);
                    
                    _parameters.TargetScope = (minion) => maxHealth1.Contains(minion);
                    break;
                case TargetType.MostMightyAlly:
                    _parameters.Count = (int)parameters[0];
                    var mighty = 
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Minions)
                            .OrderByDescending((minion => minion.Might.PersonalMight)).Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => mighty.Contains(minion);
                    break;
                case TargetType.WeakestAlly:
                    _parameters.Count = (int)parameters[0];
                    var weakest = 
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Minions)
                            .OrderBy((minion => minion.Might.PersonalMight)).Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => weakest.Contains(minion);
                    break;
                case TargetType.CurrentTarget:
                    caster = (IMinion)parameters[0];
                    _parameters.TargetScope = (minion) => minion == caster.Target;
                    break;
                case TargetType.AllEnemies:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies;
                    break;
                case TargetType.RandomEnemy:
                    _parameters.Count = (int)parameters[0];
                    var enemies = 
                        _minionPool.Minions.Where((minion) => minion.Fraction == Fraction.Enemies)
                            .ToList()
                            .Shuffle()
                            .Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => enemies.Contains(minion);
                    break;
                case TargetType.EnemyGladiators:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Gladiator;
                    break;
                case TargetType.EnemyTemplars:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Templar;
                    break;
                case TargetType.EnemyRangers:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Ranger;
                    break;
                case TargetType.EnemyAssassins:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Assassin;
                    break;
                case TargetType.EnemySpiritmasters:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Spiritmaster;
                    break;
                case TargetType.EnemySorcerers:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Sorcerer;
                    break;
                case TargetType.EnemyClerics:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Cleric;
                    break;
                case TargetType.EnemyChanters:
                    _parameters.TargetScope = (minion) => minion.Fraction == Fraction.Enemies && minion.Class == MinionClass.Chanter;
                    break;
                case TargetType.NearestEnemy:
                    _parameters.Count = (int)parameters[0];
                    caster = (IMinion)parameters[1];
                    var nearestEnemies =
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Enemies)
                            .OrderBy((minion => Distance(minion, caster)));
                    
                    int minDistanceEnemy= 0;
                    if(nearestEnemies.Any())
                        minDistanceEnemy = Distance(nearestEnemies.ToList()[0], caster);
                    
                    var nearestEnemies1 = 
                        nearestEnemies
                            .Where((minion => Distance(minion, caster) == minDistanceEnemy))
                            .ToList()
                            .Shuffle()
                            .Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => nearestEnemies1.Contains(minion);
                    break;
                case TargetType.MostWoundedEnemy:
                    _parameters.Count= (int)parameters[0];
                    var woundedEnemies = 
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Enemies)
                            .OrderBy((minion => (float)minion.Parameters.Health.Value/minion.Parameters.Health.MaxValue))
                            .Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => woundedEnemies.Contains(minion);
                    break;
                case TargetType.MostHealthiestEnemy:
                    _parameters.Count = (int)parameters[0];
                    var healthiestEnemise = 
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Enemies)
                            .OrderByDescending((minion => (float)minion.Parameters.Health.Value/minion.Parameters.Health.MaxValue))
                            .Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => healthiestEnemise.Contains(minion);
                    break;
                case TargetType.MostMightyEnemy:
                    _parameters.Count = (int)parameters[0];
                    var mightyEnemies = 
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Enemies)
                            .OrderByDescending((minion => minion.Might.PersonalMight)).Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => mightyEnemies.Contains(minion);
                    break;
                case TargetType.WeakestEnemy:
                    _parameters.Count = (int)parameters[0];
                    var weakestEnemies = 
                        _minionPool.Minions
                            .Where(minion => minion.Fraction == Fraction.Enemies)
                            .OrderBy((minion => minion.Might.PersonalMight)).Take(_parameters.Count);
                    _parameters.TargetScope = (minion) => weakestEnemies.Contains(minion);
                    break;
                case TargetType.None:
                    _parameters.TargetScope = (minion) => false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void SetDuration(float duration)
        {
            _parameters.Duration = duration;
        }

        private ICommand Build()
        {
            Action acting;
            Action canceling;
            if (_parameters.SourceRadius == null)
            {
                var commandAction = new CommandAction(_parameters.Action, _parameters.TargetScope);
                var commandCancel = new CommandAction(_parameters.Canceling, _parameters.TargetScope);
                acting = commandAction.Action;
                canceling = commandCancel.Action;
            }
            else
            {
                var commandAction = new CommandActionWithRadius(_parameters.Action, _parameters.TargetScope, _parameters.SourceRadius);
                var commandCancel = new CommandActionWithRadius(_parameters.Canceling, _parameters.TargetScope, _parameters.SourceRadius);
                acting = commandAction.Action;
                canceling = commandCancel.Action;
            }

            var internalCommand = 
                new ActiveCommand(acting, canceling, $"{_parameters.Name}", _parameters.Duration);
            ICommand command;
            switch (_parameters.Type)
            {
                case CommandType.Active:
                    command = internalCommand;
                    break;
                case CommandType.Passive:
                    // PassiveCommand passiveCommand = 
                    //     new PassiveCommand(internalCommand, _parameters.Frequency, _parameters.Duration);
                    // _worker.Prepare(passiveCommand);
                    // command = passiveCommand;
                    // break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ClearCache();
            return command;
        }

        private void ClearCache()
        {
             _parameters = new CommandParameters();
        }

        private int Distance(IMinion minion, IMinion caster)
        {
            var x = Math.Abs(minion.Position.x - caster.Position.x);
            var y = Math.Abs(minion.Position.y - caster.Position.y);
            var distance = x == 0 || y == 0 ? x + y : x + y - 1;
            if (distance == 0)
                distance = int.MaxValue;
            return distance;
        }

        public void SetRadius(int radius, IMinion caster)
        {
            _parameters.SourceRadius = (minion => Distance(minion, caster) <= radius || 
                                                  Distance(minion, caster) == int.MaxValue);
        }

        public ICommand BuildWithParameters(StringCommandParameters parameters, bool invert = true)
        {
            if (parameters.Caster != null && parameters.Caster.Fraction == Fraction.Enemies && invert)
                parameters.Target = InvertTarget(parameters.Target);
            
            SetType(CommandType.Active);
            SetAction(parameters.Action,parameters.CommandClass, parameters.ActionParameters);
            SetTarget(parameters.Target, parameters.TargetParameters);
            SetDuration(parameters.Duration);
            if(parameters.UsingRadius)
                SetRadius(parameters.Radius, parameters.Caster);
            return Build();
        }

        public static TargetType InvertTarget(TargetType target)
        {
            switch (target)
            {
                case TargetType.Self:
                    return TargetType.Self;
                case TargetType.AllAllies:
                    return TargetType.AllEnemies;
                case TargetType.RandomAlly:
                    return TargetType.RandomEnemy;
                case TargetType.AllyGladiators:
                    return TargetType.EnemyGladiators;
                case TargetType.AllyTemplars:
                    return TargetType.EnemyTemplars;
                case TargetType.AllyRangers:
                    return TargetType.EnemyRangers;
                case TargetType.AllyAssassins:
                    return TargetType.EnemyAssassins;
                case TargetType.AllySpiritmasters:
                    return TargetType.EnemySpiritmasters;
                case TargetType.AllySorcerers:
                    return TargetType.EnemySorcerers;
                case TargetType.AllyClerics:
                    return TargetType.EnemyClerics;
                case TargetType.AllyChanters:
                    return TargetType.EnemyChanters;
                case TargetType.NearestAlly:
                    return TargetType.NearestEnemy;
                case TargetType.MostWoundedAlly:
                    return TargetType.MostWoundedEnemy;
                case TargetType.MostHealthiestAlly:
                    return TargetType.MostHealthiestEnemy;
                case TargetType.MostMightyAlly:
                    return TargetType.MostMightyEnemy;
                case TargetType.WeakestAlly:
                    return TargetType.WeakestEnemy;
                case TargetType.CurrentTarget:
                    return TargetType.CurrentTarget;
                case TargetType.AllEnemies:
                    return TargetType.AllAllies;
                case TargetType.RandomEnemy:
                    return TargetType.RandomAlly;
                case TargetType.EnemyGladiators:
                    return TargetType.AllyGladiators;
                case TargetType.EnemyTemplars:
                    return TargetType.AllyTemplars;
                case TargetType.EnemyRangers:
                    return TargetType.AllyRangers;
                case TargetType.EnemyAssassins:
                    return TargetType.AllyAssassins;
                case TargetType.EnemySpiritmasters:
                    return TargetType.AllySpiritmasters;
                case TargetType.EnemySorcerers:
                    return TargetType.AllySorcerers;
                case TargetType.EnemyClerics:
                    return TargetType.AllyClerics;
                case TargetType.EnemyChanters:
                    return TargetType.AllyChanters;
                case TargetType.NearestEnemy:
                    return TargetType.NearestAlly;
                case TargetType.MostWoundedEnemy:
                    return TargetType.MostWoundedAlly;
                case TargetType.MostHealthiestEnemy:
                    return TargetType.MostHealthiestAlly;
                case TargetType.MostMightyEnemy:
                    return TargetType.MostMightyAlly;
                case TargetType.WeakestEnemy:
                    return TargetType.WeakestAlly;
                case TargetType.None:
                    return TargetType.None;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }
        }
    }

    public class CommandParameters
    {
        public CommandType Type;
        public Func<Result> Action;
        public Func<Result> Canceling;
        public Func<IMinion, bool> TargetScope;
        public Func<IMinion, bool> SourceRadius;
        public float Duration = float.MaxValue;
        public int Count;
        public string Name;
    }

    public class CommandAction
    {
        private Func<Result> _action;
        private Func<IMinion, bool> _targetScope;

        public CommandAction(Func<Result> action, Func<IMinion, bool> targetScope)
        {
            _action = action;
            _targetScope = targetScope;
        }

        public void Action()
        {
            _action.Invoke().For(_targetScope).Now();
        }
    }
    
    public class CommandActionWithRadius
    {
        private Func<Result> _action;
        private Func<IMinion, bool> _targetScope;
        private Func<IMinion, bool> _sourceRadius;

        public CommandActionWithRadius(Func<Result> action, Func<IMinion, bool> targetScope, Func<IMinion, bool> sourceRadius)
        {
            _action = action;
            _targetScope = targetScope;
            _sourceRadius = sourceRadius;
        }

        public void Action()
        {
            _action.Invoke().For(_targetScope).For(_sourceRadius).Now();
        }
    }
}