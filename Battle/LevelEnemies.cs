using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Battle
{
    [CreateAssetMenu(fileName = nameof(LevelEnemies), menuName = nameof(LevelEnemies))]
    public class LevelEnemies : ScriptableObject
    {
        [SerializeField] private List<Wave> _eniemiesWaves;
        [SerializeField] private GameObject[] _bossWaves;

        public GameObject BossWave
        {
            get
            {
                int index = PlayerPrefs.GetInt("level");
                return _bossWaves[index];
            }
        }

        public GameObject GetRandomWave()
        {
            int index = PlayerPrefs.GetInt("level");
            return _eniemiesWaves[index].EniemiesWave.GetRandom();
        }
    }

    [Serializable]
    public struct Wave
    {
        [SerializeField] public List<GameObject> EniemiesWave;
    }
}