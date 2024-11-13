using System;
using System.Collections.Generic;
using System.Linq;
using Fight.Attack;
using Fight.Fractions;
using Model.Commands;
using Model.Commands.Types;
using Realization.States.CharacterSheet;
using UnityEngine;

namespace Units
{
    public class CommandInvoker : IDisposable
    {
        public static bool WorkingFeature = true;
        
        private CommandFacade _commandFacade;
        private Skill[] _skills;
        private bool _working = true;
        private bool _passiveWorking = true;
        private EnergyStorage _energyStorage;
        private IMinion _caster;
        private List<ICommand> _passives = new();

        public CommandInvoker(Skill[] skills, CommandFacade commandFacade, EnergyStorage energyStorage, IMinion caster)
        {
            _caster = caster;
            _energyStorage = energyStorage;
            _skills = skills;
            _commandFacade = commandFacade;
            _energyStorage.Filled += TryPerform;
        }

        public bool Working => _working;

        public bool OnlyPassives
        {
            get
            {
                return _skills.All(skill => !skill.SkillValue.Contains("Active"));
            }
        }

        public void FreePerform(string skill)
        {
            var command = _commandFacade.MakeCommand(skill, _caster);
            command.Perform();
            _passives.Add(command);
        }

        public void PerformPassives()
        {
            
            if (_passiveWorking == false || WorkingFeature == false)
                return;
            
            foreach (var skill in _skills)
            {
                if(skill.SkillValue == "-" || skill.SkillValue.Contains("Passive") == false)
                    continue;
                
                var command = _commandFacade.MakeCommand(skill.SkillValue, _caster);
                command.Perform();
                _passives.Add(command);
                Debug.Log($"Perform {skill.SkillValue}");
            }
        }

        public void TryPerform()
        {
            if(_working == false || _energyStorage.EnergyValue != _energyStorage.MaxValue || WorkingFeature == false)
                return;

            foreach (var skill in _skills)
            {
                if(skill.SkillValue == "-" || skill.SkillValue.Contains("Passive"))
                    continue;
                
                var command = _commandFacade.MakeCommand(skill.SkillValue, _caster);
                command.Perform();
                _passives.Add(command);
                _energyStorage.EnergyValue = -_energyStorage.EnergyValue;
                Debug.Log($"Perform {skill.SkillValue}");
            }
        }

        public void Disable()
        {
            _working = false;
        }

        public void Activate()
        {
            _working = true;
        }
        
        public void DisablePassives()
        {
            _passiveWorking = false;
        }

        public void ActivatePassives()
        {
            _passiveWorking = true;
        }

        public void Dispose()
        {
            Debug.Log($"Dispose command invoker");
            UndoPassives();
            _energyStorage.Filled -= TryPerform;
        }

        public void UndoPassives()
        {
            foreach (var passive in _passives)
            {
                passive.Undo();
            }
            _passives.Clear();
        }
    }
}