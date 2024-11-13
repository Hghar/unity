using AssetStore.HeroEditor.Common.CharacterScripts;
using Fight.Attack;
using UnityEngine;

namespace Animators
{
    public class AttackAnimator : MonoBehaviour
    {
        [SerializeField] private Attacker _attacker;
        
        private Character _character;

        private void OnEnable()
        {
            _attacker.AttackedLegacy += OnAttackedLegacy;
        }

        private void OnDisable()
        {
            _attacker.AttackedLegacy -= OnAttackedLegacy;
        }

        private void OnAttackedLegacy()
        {
            _character.Slash();
        }

        public void SetCharacter(Character character)
        {
            _character = character;
        }
    }
}