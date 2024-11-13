using Infrastructure.Shared.Extensions;
using Model.Economy;
using Realization.States.CharacterSheet;
using Units;
using UnityEngine;

namespace Realization.Configs
{
    [CreateAssetMenu(fileName = nameof(ShopItemConfig), menuName = "Configs/ShopItem", order = 0)]
    public class ShopItemConfig : ScriptableObject
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private MinionClass _name;
        [SerializeField] private Currency _currency = Currency.Gold;
        
        public Sprite Sprite => _sprite;
        public string Name => _name.ToString();
        public Currency Currency => _currency;
    }
}