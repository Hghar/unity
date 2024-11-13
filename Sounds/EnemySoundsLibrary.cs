using System;
using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Sounds
{
    [CreateAssetMenu(fileName = nameof(EnemySoundsLibrary), menuName = "Configs/EnemySoundsLibrary", order = 0)]
    public class EnemySoundsLibrary : ScriptableObject
    {
        [SerializeField] private List<RaceSoundPair> _startFightSounds;

        public AudioClip GetStartFightSound(Race race)
        {
            return _startFightSounds.Find(pair => pair.Race == race).AudioClip;
        }
    }

    [Serializable]
    public class RaceSoundPair
    {
        [SerializeField] private Race _race;
        [SerializeField] private AudioClip _audioClip;

        public Race Race => _race;
        public AudioClip AudioClip => _audioClip;
    }
}