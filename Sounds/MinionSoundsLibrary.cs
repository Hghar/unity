using System;
using System.Collections.Generic;
using Extensions;
using Units;
using UnityEngine;

namespace Sounds
{
    [CreateAssetMenu(fileName = nameof(MinionSoundsLibrary), menuName = "Configs/MinionSoundsLibrary", order = 0)]
    public class MinionSoundsLibrary : ScriptableObject
    {
        [SerializeField] private List<ClassSoundPair> _startSounds;
        [SerializeField] private List<ClassSoundPair> _attackSounds;
        [SerializeField] private List<AudioClip> _deathClips;

        public AudioClip GetStartSound(MinionClass minionClass)
        {
            return _startSounds.Find(pair => pair.MinionClass == minionClass).AudioClip;
        }

        public AudioClip GetAttackSound(MinionClass minionClass)
        {
            return _attackSounds.Find(pair => pair.MinionClass == minionClass).AudioClip;
        }

        public AudioClip GetRandomDeathSound()
        {
            return _deathClips.GetRandom();
        }
    }

    [Serializable]
    public class ClassSoundPair
    {
        [SerializeField] private MinionClass _minionClass;
        [SerializeField] private AudioClip _audioClip;

        public MinionClass MinionClass => _minionClass;
        public AudioClip AudioClip => _audioClip;
    }
}