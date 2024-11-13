using System;
using AssetStore.HeroEditor.Common.CharacterScripts;
using Movement;
using Units.Ai;
using UnityEngine;

namespace Animators
{
    public class MovementAnimator : MonoBehaviour
    {
        private Character _character;
        private Vector3 _oldPosition;

        public void Ready()
        {
            _character.SetState(CharacterState.Ready);
        }

        public void Run()
        {
            _character.SetState(CharacterState.Run);
        }

        public void SetCharacter(Character character)
        {
            _character = character;
        }
    }
}