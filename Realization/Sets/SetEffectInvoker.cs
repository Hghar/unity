using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Fight.Fractions;
using Model.Commands;
using Model.Commands.Types;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;

namespace Realization.Sets
{
    public class SetEffectInvoker : IDisposable
    {
        public static bool WorkingFeature = true;
        
        private IBattleStartPublisher _battleStartPublisher;
        private IBattleFinishPublisher _battleFinishPublisher;
        private CommandFacade _commandFacade;
        private MinionFactory _minionFactory;
        private CharacterConfig _config;
        private List<ICommand> _activatedCommands = new();

        public SetEffectInvoker(
            IBattleStartPublisher battleStartPublisher,
            IBattleFinishPublisher battleFinishPublisher,
            CommandFacade commandFacade,
            CharacterConfig config,
            MinionFactory minionFactory)
        {
            _config = config;
            _minionFactory = minionFactory;
            _commandFacade = commandFacade;
            _battleStartPublisher = battleStartPublisher;
            _battleFinishPublisher = battleFinishPublisher;
        }

        public void Init()
        {
            _battleStartPublisher.BattleStarted += ActivateSets;
            _battleFinishPublisher.BattleFinished += DisableSets;
        }

        public void Dispose()
        {
            _battleStartPublisher.BattleStarted -= ActivateSets;
            _battleFinishPublisher.BattleFinished -= DisableSets;
            _commandFacade?.Dispose();
        }

        private void DisableSets()
        {
            foreach (var command in _activatedCommands)
            {
                command.Undo();
            }
            _activatedCommands.Clear();
        }

        private void ActivateSets()
        {
            if(WorkingFeature == false)
                return;
            
            List<string> activatedSetUids = new();
            Dictionary<MinionClass, int> classCounts = CalculateMinionsByClass();
            
            foreach (var minionCount in classCounts)
            {
                activatedSetUids.Add(FindMaxSetEffect(minionCount.Key, minionCount.Value));
            }

            foreach (var setUid in activatedSetUids)
            {
                if(setUid == "-")
                    continue;

                string[] commands = FindSkills(setUid);
                foreach (var commandString in commands)
                {
                    var command = _commandFacade.MakeCommand(commandString);
                    command.Perform();
                    _activatedCommands.Add(command);
                }
            }
        }

        private string[] FindSkills(string setUid)
        {
            var skills = _config.Skills.FindAll((skill => skill.Uid == setUid));
            List<string> actions = new();
            foreach (var skill in skills)
            {
                actions.Add(skill.SkillValue);
            }

            return actions.ToArray();
        }

        private string FindMaxSetEffect(MinionClass minionClass, int minionCount)
        {
            var sets = _config.CharacterSets
                .Where((set => set.Class == minionClass))
                .OrderByDescending((set => set.UnitCount));

            foreach (var set in sets)
            {
                if (set.UnitCount <= minionCount)
                {
                    return set.Effect;
                }
            }

            return "-";
        }

        private string FindInfoSetEffect(MinionClass minionClass, int minionCount)
        {
            var sets = _config.CharacterSets
                .Where((set => set.Class == minionClass))
                .OrderByDescending((set => set.TextEng));

            foreach (var set in sets)
            {
                if (set.UnitCount <= minionCount)
                {
                    return set.TextEng;
                }
            }

            return "-";
        }

        private Dictionary<MinionClass, int>  CalculateMinionsByClass()
        {
            Dictionary<MinionClass, List<string>> classUids = new();
            Dictionary<MinionClass, int> classCounts = new();
            
            foreach (var minion in _minionFactory.Minions)
            {
                if (minion.Fraction == Fraction.Minions)
                {
                    if (classUids.ContainsKey(minion.Class) == false)
                    {
                        classUids.Add(minion.Class, new List<string>());
                        classCounts.Add(minion.Class, 0);
                    }

                    if (classUids[minion.Class].Contains(minion.Uid) == false)
                    {
                        classUids[minion.Class].Add(minion.Uid);
                        classCounts[minion.Class] += 1;
                    }
                }
            }

            return classCounts;
        }
    }
}