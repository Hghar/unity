using System.Collections.Generic;
using Infrastructure.Services.WindowService;
using UnityEngine;

namespace Realization.States.CharacterSheet
{
    [CreateAssetMenu(menuName = "Static Data/PopUp static data", fileName = "PopupStaticConfig")]
    public class PopupStaticConfig : ScriptableObject
    {
        public List<PopUpWindowData> Configs;
    }
}