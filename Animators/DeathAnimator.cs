using AssetStore.HeroEditor.Common.CharacterScripts;
using Fight;
using UnityEngine;

namespace Animators
{
    public class DeathAnimator : MonoBehaviour
    {
        [SerializeField] private Mortality _mortality;
        
        private Character _character;

        private void OnEnable()
        {
            _mortality.Dying += OnDying;
        }

        private void OnDisable()
        {
            _mortality.Dying -= OnDying;
        }

        private void OnDying()
        {
            bool isFirstDeathType = Random.Range(0, 10) > 4;

            if (isFirstDeathType)
                _character.SetState(CharacterState.DeathF);
            else
                _character.SetState(CharacterState.DeathB);
        }

        public void SetCharacter(Character character)
        {
            _character = character;
        }
    }
}