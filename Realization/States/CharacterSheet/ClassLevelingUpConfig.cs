using System.Collections.Generic;
using Parameters;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [CreateAssetMenu(fileName = "ClassLevelingUpConfig", menuName = "Configs/ClassLevelingUp", order = 0)]
    public class ClassLevelingUpConfig : ScriptableObject
    {
        public List<LevelingUpConfig> Configs;

        public void UpdateConfig(List<LevelingUpConfig> levelingUpConfigs)
        {
            Configs = new List<LevelingUpConfig>(levelingUpConfigs);
        }
    }
}