using System;
using Fight.Attack;
using Units;
using UnityEngine;

namespace Sounds
{
    public class MinionAttackSoundsPlayer : MonoBehaviour
    {
        [SerializeField] private Attacker _attacker;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MinionSoundsLibrary _soundsLibrary;
        [SerializeReference] private IMinion _minion;

        private void OnEnable()
        {
            _attacker.AttackedLegacy += OnAttackedLegacy;
        }

        private void OnDisable()
        {
            _attacker.AttackedLegacy -= OnAttackedLegacy;
        }

        private void OnAttackedLegacy()
        {
            try
            {
                if (_minion.Class != MinionClass.Cleric)
                {
                    AudioClip audioClip = _soundsLibrary.GetAttackSound(_minion.Class);
                    _audioSource.PlayOneShot(audioClip);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }
    }
}