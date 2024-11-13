using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.DevTools
{
    public class EnergyDevTool : MonoBehaviour
    {
        [SerializeField] private ParamsMenu _paramsMenu;

        [SerializeField] private Button _fillEnergy;
        [SerializeField] private Toggle _enableSkill;
        [SerializeField] private TMP_InputField _addPercents;
        [SerializeField] private TMP_Text _attributes;

        private void Awake()
        {
            _fillEnergy.onClick.AddListener(FillMana);
            _enableSkill.onValueChanged.AddListener(SwitchSilence);
            _addPercents.onSubmit.AddListener(AddPercents);
        }

        private void LateUpdate()
        {
            if(_paramsMenu.SelectedMinion == null)
                return;

            _enableSkill.onValueChanged.RemoveListener(SwitchSilence);
            _enableSkill.isOn = _paramsMenu.SelectedMinion.Silenced;
            _enableSkill.onValueChanged.AddListener(SwitchSilence);
        }

        private void Update()
        {
            if(_paramsMenu.SelectedMinion == null)
                return;

            var maxHealth = _paramsMenu.SelectedMinion.Parameters.Health.MaxValue;
            var attackSpeed = _paramsMenu.SelectedMinion.Parameters.Cooldown.Value;
            var aggression = _paramsMenu.SelectedMinion.Aggression;
            var critChance = _paramsMenu.SelectedMinion.Parameters.ChanceOfCriticalDamage.Value;
            var critMultiplier = _paramsMenu.SelectedMinion.Parameters.CriticalDamageMultiplier.Value;
            var dodgeChance = _paramsMenu.SelectedMinion.Parameters.Agility.Value;
            var shields = _paramsMenu.SelectedMinion.Shields;
            _attributes.text = $"max health: {maxHealth} attack speed:{attackSpeed} agression: {aggression} crit chance: {critChance} " +
                          $"crit multiplier: {critMultiplier} dodge chance: {dodgeChance} shields: {shields}";
        }

        private void AddPercents(string arg0)
        {
            var value = _paramsMenu.SelectedMinion.Parameters.Energy.MaxValue*float.Parse(arg0)/100;
            _paramsMenu.SelectedMinion.AddEnergy((int)value);
        }

        private void SwitchSilence(bool arg0)
        {
            if (arg0)
            {
                _paramsMenu.SelectedMinion.HardSilence();
            }
            else
            {
                _paramsMenu.SelectedMinion.HardUnsilence();
            }
        }

        private void FillMana()
        {
            _paramsMenu.SelectedMinion.AddEnergy(10000);
        }
    }
}