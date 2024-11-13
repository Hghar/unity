using Battle;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Enemy = Battle.Enemy;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class WinButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}