using System.Collections.Generic;
using Parameters;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [CreateAssetMenu(fileName = "New GeneralLevelingUp Config", menuName = "Configs/GeneralLevelingUp", order = 0)]
    public class GeneralLevelingConfig : ScriptableObject
    {
        public List<GeneralLevelingUpConfig> Configs;

        public void UpdateConfig(List<GeneralLevelingUpConfig> generalLevelingUp)
        {
            Configs = new List<GeneralLevelingUpConfig>( generalLevelingUp);
        }
    }
}