using System.Collections.Generic;
using Infrastructure.Services.WindowService;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [CreateAssetMenu(menuName = "Static Data/Window static data", fileName = "WindowStaticData")]
    public class WindowStaticConfig : ScriptableObject
    {
        public List<WindowData> Configs;
    }
}