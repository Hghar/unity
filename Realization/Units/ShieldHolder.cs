using System.Collections.Generic;
using System.Linq;
using Model.Commands.Parts;
using UnityEngine;

namespace Realization.General
{
    public class ShieldHolder
    {
        private Dictionary<CommandClass, int> _shields = new();
        private Dictionary<CommandClass, int> _shieldStartValues = new();
        private Dictionary<CommandClass, int> _shieldCounts = new();

        public int Summ
        {
            get
            {
                var summ = 0;
                foreach (var shield in _shields)
                {
                    summ += shield.Value;
                }

                return summ;
            }
        }

        public int Count
        {
            get
            {
                var count = 0;
                foreach (var shield in _shieldCounts)
                {
                    count += shield.Value;
                }

                return count;
            }
        }

        public void Add(CommandClass commandClass, int shieldValue)
        {
            if (_shields.ContainsKey(commandClass))
            {
                if(_shieldCounts.ContainsKey(commandClass) == false)
                    _shieldCounts.Add(commandClass, 1);
                _shields[commandClass] += shieldValue;
                return;
            }
            
            _shields.Add(commandClass, shieldValue);
            _shieldStartValues.Add(commandClass, shieldValue);
            if(_shieldCounts.ContainsKey(commandClass) == false)
            {
                _shieldCounts.Add(commandClass, 0);
            }
            _shieldCounts[commandClass] += 1;
        }
        
        public void Remove(CommandClass commandClass)
        {
            if(HasShield(commandClass) == false &&  _shields.ContainsKey(commandClass) && _shields[commandClass] > 0)
                return;

            _shieldCounts[commandClass] -= 1;
            if (_shieldCounts[commandClass] == 0)
            {
                _shields.Remove(commandClass);
                _shieldStartValues.Remove(commandClass);
            }
        }
        
        public bool HasShield(CommandClass commandClass)
        {
            if (commandClass.Duration == float.MaxValue)
                return false;
            
            return _shields.Any((pair => pair.Key.Equals(commandClass)));
        }

        public void UpdateShield(CommandClass commandClass, int shieldValue)
        {
            _shields[commandClass] = _shieldStartValues[commandClass];
            _shieldCounts[commandClass] += 1;
        }

        public int TakeDamage(float value)
        {
            foreach (var shield in _shields.ToArray())
            {
                var restDamage = shield.Value - value;
                _shields[shield.Key] = (int)Mathf.Max(0, restDamage);
                if (restDamage < 0)
                {
                    value = Mathf.Abs(restDamage);
                    Remove(shield.Key);
                }
                else
                {
                    return 0;
                }
            }
            return (int)value;
        }

        public void RemoveAll()
        {
            _shields = new();
            _shieldStartValues = new();
            _shieldCounts = new();
        }
    }
}