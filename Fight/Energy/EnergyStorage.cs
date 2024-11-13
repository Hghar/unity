using System;
using Fight.Attack;
using Parameters;
using System.Collections;
using System.Collections.Generic;
using Fight.Damaging;
using UnityEngine;
using Units;

namespace Fight.Attack
{
    public class EnergyStorage : MonoBehaviour
    {
        private const float ENERGY_PER_HIT = 15;
        public static bool Working = true;

        public static bool IsWorking = true;

        private bool isActiveEnergy = true;

        [SerializeField] private ControlBars _controlBars;
        [SerializeField] private Attacker _attacker;
        [SerializeField] private Damageable _damageable;
        
        private Health _health;
        private Energy _energy;

        private float _maxEnergyValue = 100;

        private float _energyValue = 0;

        private float _maxHealthValue = 100;

        public float EnergyValue
        {
            get => (int)_energyValue;
            set
            {
                _energyValue = (value > _maxEnergyValue) ? _maxEnergyValue : value < 0 ? 0 : value;

                _controlBars.UpdateEnergyBar(_energyValue, _maxEnergyValue);

                if (IsWorking == true)
                {
                    if (_energyValue == MaxValue && Working && _energy.MaxValue > 1)
                        Filled?.Invoke();
                }
                else
                {
                    if (_energyValue == MaxValue)
                        Filled?.Invoke();
                }

            }
        }

        public float MaxValue => _maxEnergyValue;
        
        public event Action Filled;

        private void FixedUpdate()
        {
            if (_energy.MaxValue > 1)
                _controlBars.SetEnergyBarActive(Working);
        }

        public void SetConfig(IUnitParameters config, Health health)
        {
            _health = health;

            _energy = config.Energy;

            _maxEnergyValue = _energy.MaxValue;

            _maxHealthValue = config.Health.MaxValue;
            
            _attacker.Attacked += AddEnergy;
            _attacker.Attacked += AddEnergyToTarget;
            

            if (_energy.MaxValue <= 1)
            {
                _controlBars.SetEnergyBarActive(false);
                isActiveEnergy = false;
            }

            _controlBars.UpdateEnergyBar(_energyValue, _maxEnergyValue);

            // _attacker.Healing += AddEnergy;

            // health.DecreasedBy += AddEnergy;
        }

        private void AddEnergy(IMinion arg1, float arg2, bool arg3, int difference)
        {
            EnergyValue += ENERGY_PER_HIT;
            _controlBars.UpdateEnergyBar(_energyValue, _maxEnergyValue);
        }

        private void AddEnergyToTarget(IMinion target, float damage, bool arg2, int difference)
        {
            target.AddEnergy(CalculateEnergyFromDamage(target, difference), false);
        }
        
        private void AddEnergy()
        {
            EnergyValue += ENERGY_PER_HIT;
            _controlBars.UpdateEnergyBar(_energyValue, _maxEnergyValue);
        }
        
        private float CalculateEnergyFromDamage(IMinion target, float damage)
        {
            float damagePer = damage / ((float)target.Parameters.Health.MaxValue * 2f);

            float addedEnergy = target.Parameters.Energy.MaxValue * damagePer;
            
            if(addedEnergy < 1)
            {
                addedEnergy = 1;
            }
            
            return (int)addedEnergy;
        }

        private void OnDestroy()
        {
            _attacker.Attacked -= AddEnergy;
            _attacker.Attacked -= AddEnergyToTarget;
            // _attacker.Healing -= AddEnergy;

            // _health.DecreasedBy -= AddEnergy;
        }

        public void Empty()
        {
            _energyValue = 0;
            _controlBars.UpdateEnergyBar(_energyValue, _maxEnergyValue);
        }
    }
}

