using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.Shops
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _experience;//todo
        [SerializeField] private TMP_Text _level;//todo
        [SerializeField] private SpriteRenderer _classIcon;
        [SerializeField] private TMP_Text _class;
        [SerializeField] private TMP_Text _might;
        [SerializeField] private TMP_Text _power;
        [SerializeField] private TMP_Text _attackSpeed;
        [SerializeField] private TMP_Text _armor;
        [SerializeField] private Button _buy;

        public event Action<object> Clicked;

        private void Awake()
        {
            _buy.onClick.AddListener((() => Clicked.Invoke(this)));
        }

        private void OnDestroy()
        {
            Clicked = null;
        }
    }
}