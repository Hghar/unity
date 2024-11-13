using System;
using System.Collections.Generic;
using System.Linq;
using AssetStore.HeroEditor.Common.CharacterScripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fight
{
    public static class CharacterHelper
    {
        private const string _path = "Characters";

        public static Character GetVisual(Transform characterParent, string prefabName)
        {
            Character character =  Resources.Load<Character>($"{_path}/{prefabName}");
            if(character == null) Debug.LogError($"Can't find visual '{_path}/{prefabName}'");
            return Object.Instantiate(character, characterParent);
        }
    }
}