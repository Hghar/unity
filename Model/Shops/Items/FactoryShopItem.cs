// using Units;
// using System;
// using GridMap.Behaviours;
// using Infrastructure.Shared.Factories;
// using Model.Economy;
// using UnityEngine;
//
// namespace Model.Shops.Items
// {
//     public class FactoryShopItem : IShopItem
//     {
//         private readonly IFactory _factory;
//         private readonly IStorage _storage;
//         private readonly Currency _currency;
//         private readonly string _name;
//         private readonly int _price;
//         private readonly Sprite _sprite;
//
//         public event Action Bought;
//
//         public string Name => _name;
//         public int Price => _price;
//         public Sprite Sprite => _sprite;
//
//         public FactoryShopItem(IFactory factory, IStorage storage, Currency currency, string name,
//             int price, Sprite sprite)
//         {
//             _factory = factory;
//             _storage = storage;
//             _currency = currency;
//             _name = name;
//             _price = price;
//             _sprite = sprite;
//         }
//
//         public void TryBuy()
//         {
//             if (MinionFactory.Units.Count >= 16)
//                 return;
//                 
//             if (_storage.TrySpendResource(_currency, _price))
//             {
//                 _factory.Create();
//                 Bought?.Invoke();
//             }
//         }
//     }
// }