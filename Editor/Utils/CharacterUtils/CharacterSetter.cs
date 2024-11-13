using System;
using AssetStore.HeroEditor.Common.CharacterScripts;
using NaughtyAttributes;
using UnityEngine;

namespace Editor.Utils.CharacterUtils
{
    [RequireComponent(typeof(Character))]
    public class CharacterSetter : MonoBehaviour
    {
        [SerializeField] private Character _character;
        [SerializeField] private string _json;

        [Button]
        private void UpdateJson()
        {
            _character.FromJson(_json);
        }
    }
}