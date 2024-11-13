// using System;
// using Firebase.Analytics;
// using GridMap.Behaviours;
// using Units;
// using UnityEngine;
//
// namespace Model.Shops.Items
// {
//     public class UnitItem : IShopItem, IDisposable
//     {
//         private readonly IShopItem _item;
//         private readonly IGridBehaviour _gridBehaviour;
//
//         public event Action Bought;
//
//         public string Name => _item.Name;
//         public int Price => _item.Price;
//         public Sprite Sprite => _item.Sprite;
//
//         public UnitItem(IShopItem item, IGridBehaviour gridBehaviour)
//         {
//             _item = item;
//             _gridBehaviour = gridBehaviour;
//             _item.Bought += OnBought;
//         }
//
//         public void TryBuy()
//         {
//             if (MinionFactory.Units.Count >= 16)
//                 return;
//             
//             if (_gridBehaviour.HasEmptyTiles)
//                 _item.TryBuy();
//         }
//
//         public void Dispose()
//         {
//             _item.Bought -= OnBought;
//         }
//
//         private void OnBought()
//         {
//             string message = "unit_bought_";
//             message += "l" + PlayerPrefs.GetInt("level");
//             FirebaseAnalytics.LogEvent(message);
//
//             Bought?.Invoke();
//         }
//     }
// }