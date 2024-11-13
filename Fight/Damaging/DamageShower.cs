using AssetStore.HeroEditor.Common.CharacterScripts;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Fight.Damaging
{
    public class DamageShower : MonoBehaviour
    {
        [SerializeField] private Health _health;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DisappearingText _textPrefab;
        [SerializeField] private Color _color;
        [SerializeField] private Vector2 _randomizedPositionOffset;

        private void OnEnable()
        {
            _health.DecreasedBy += OnHealthDecreased;

            //_spawnPoint = this.gameObject.transform.GetComponentInChildren<CharacterBodySculptor>().MeleeWeapon[0];
        }

        private void OnDisable()
        {
            _health.DecreasedBy -= OnHealthDecreased;
        }

        private void OnHealthDecreased(int delta)
        {
            float offsetX = Random.Range(-_randomizedPositionOffset.x, _randomizedPositionOffset.x);
            float offsetY = Random.Range(-_randomizedPositionOffset.y, _randomizedPositionOffset.y);
            Vector3 offset = new Vector2(offsetX, offsetY);

            DisappearingText text = Instantiate(_textPrefab, _spawnPoint.position + offset, Quaternion.identity);
            text.Init(delta.ToString(), _color);
        }
    }
}