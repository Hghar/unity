using Model.Economy;
using UnityEngine;

namespace UnitSelling
{
    [CreateAssetMenu(fileName = nameof(UnitSellingConfig), menuName = "Configs/" + nameof(UnitSellingConfig),
        order = 0)]
    public class UnitSellingConfig : ScriptableObject, IUnitSellingConfig
    {
        [SerializeField] private CurrencyValuePair _unitPrice;

        public Currency UnitPriceCurrency => _unitPrice.Currency;
        public int[] UnitPriceValue => _unitPrice.Value;
    }
}