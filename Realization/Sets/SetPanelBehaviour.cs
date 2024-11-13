using System;
using System.Collections.Generic;
using System.Linq;
using Parameters;
using Realization.States.CharacterSheet;
using TMPro;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.Sets
{
    public class SetPanelBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _gladiator;
        [SerializeField] private TMP_Text _templar;
        [SerializeField] private TMP_Text _cleric;
        [SerializeField] private TMP_Text _chanter;
        [SerializeField] private TMP_Text _sorcerer;
        [SerializeField] private TMP_Text _spiritmaster;
        [SerializeField] private TMP_Text _ranger;
        [SerializeField] private TMP_Text _assassin;

        private List<CharacterSet> _sets;

        [Inject]
        private void Construct(CharacterConfig config)
        {

            _sets = config.CharacterSets;
        }

        public void Set(MinionClass minionClass, int count)
        {
            int max = MaxCount(minionClass);
            
            switch (minionClass)
            {
                case MinionClass.Gladiator:
                    _gladiator.text = $"{count}/{max}";
                    break;
                case MinionClass.Templar:
                    _templar.text = $"{count}/{max}";
                    break;
                case MinionClass.Cleric:
                    _cleric.text = $"{count}/{max}";
                    break;
                case MinionClass.Chanter:
                    _chanter.text = $"{count}/{max}";
                    break;
                case MinionClass.Sorcerer:
                    _sorcerer.text = $"{count}/{max}";
                    break;
                case MinionClass.Spiritmaster:
                    _spiritmaster.text = $"{count}/{max}";
                    break;
                case MinionClass.Ranger:
                    _ranger.text = $"{count}/{max}";
                    break;
                case MinionClass.Assassin:
                    _assassin.text = $"{count}/{max}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(minionClass), minionClass, null);
            }
        }

        private int MaxCount(MinionClass minionClass)
        {
            return _sets.Count((set => set.Class == minionClass));
        }
    }
}