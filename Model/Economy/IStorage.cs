using System;
using System.Collections.Generic;
using System.Linq;
using Model.Economy.Resources;
using Units;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model.Economy
{
    public interface IStorage
    {
        PlayerProgress PlayerProgress { get; set; }

        int GetResourceValue(Currency currency);
        bool AddResource(Currency currency, int addableAmount, bool animate = true);
        void SpendResource(Currency currency, int price, bool animate = true);
        bool HaveResource(Currency currency, int price);
        void SetResource(Currency currency, int value);
    }

    [Serializable]
    public class PlayerProgress
    {
        public Bioms Bioms;
        public WorldData WorldData;
        public Tutorial TutorialData;
        public Analytics Analytics;
        public CoreUpgrades CoreUpgrades;
        public PlayerProgress()
        {
            CoreUpgrades = new CoreUpgrades();
            WorldData = new WorldData();
            Bioms = new Bioms();
            TutorialData = new Tutorial();
            Analytics = new Analytics();
        }
    }

    [Serializable]

    public class MetaLeveling
    {
        
    }

    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
    {
        public Dictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();

        [SerializeField] private TKey[] _keys;
        [SerializeField] private TValue[] _values;

        public void OnBeforeSerialize()
        {
            _keys = Dictionary.Keys.ToArray();
            _values = Dictionary.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < _keys.Length; i++)
                Dictionary.Add(_keys[i], _values[i]);
        }
    }
    [Serializable]
    public class MinionsStatsDictionary : SerializableDictionary<ClassParent, int>
    {
        public int GetLevel(ClassParent id) => 
                Dictionary.ContainsKey(id) ? Dictionary[id] : 0;


        public void SetLevel(ClassParent id, int level)
        {
            var remove = Dictionary.Remove(id);
            Dictionary.Add(id,level);
        }
    }

    [Serializable]
    public class CoreUpgrades
    {
        public int CurrentGeneralLevel;
        public MinionsStatsDictionary Stats;

        public CoreUpgrades()
        {
            Stats = new MinionsStatsDictionary();
        }
    }
    [Serializable]
    public class Analytics
    {
        public DateTime RegisterTime;
        public double SpentDays => (DateTime.Now - RegisterTime).TotalDays;
        public DateTime LevelStartTime;
    }
    [Serializable]
    public class Tutorial
    {
        [SerializeField] public bool Started;
        [SerializeField] public int Key;
    }

    [Serializable]
    public class ShopData
    {
        public int Level;
    }
    
    [Serializable]
    public class WorldData
    {
        public CurrencyData CurrencyData;

        public WorldData()
        {
            CurrencyData = new CurrencyData();
        }
    }

    [Serializable]
    public class CurrencyData //:ISerializationCallbackReceiver
    {
        [SerializeField] public Resource[] IResources;
        public int GoldValue => IResources.First(x => x.Currency == Currency.Gold).Value;
        public int TokenValue => IResources.First(x => x.Currency == Currency.Hard).Value;
        public int CrystalValue => IResources.First(x => x.Currency == Currency.Crystals).Value;
        public int MetaGoldValue => IResources.First(x => x.Currency == Currency.MetaGold).Value;
        public event Action<int> GoldPlus;
        public event Action<int> GoldMinus;

        public void PlusCoreGold(int count)
        {
            GoldPlus?.Invoke(count);
        }
        public void MinusGold(int count)
        {
            GoldMinus?.Invoke(count);
        }
        
        /*[SerializeField] private Resource[] resources;
        public void OnBeforeSerialize()
        {
            resources = Array.ConvertAll(IResources, x => x as Resource);
        }

        public void OnAfterDeserialize()
        {
            IResources = new Resource[resources.Length];
            resources.CopyTo(IResources,0);
        }*/

        public bool CanPay(Currency gold, int price)
        {
            var resource = IResources.First(x => x.Currency == gold);
            return resource.CanSubtract(price);
        }

        public void Pay(Currency gold,int price)
        {
            var resource = IResources.First(x => x.Currency == gold);
            resource.TrySubtract(price);
        }

        public void Add(Currency crystals, int tokens)
        {
            var resource = IResources.First(x => x.Currency == crystals);
            var resourceData = resource as Resource;
            resourceData.Add(tokens);
        }
    }

    [Serializable]
    public class Bioms
    {
        public List<Biom> Opened = new List<Biom>();
        public Biom SelectedBiom;
        public int CurrentRoom => SelectedBiom.CurrentRoom;
    }

    [Serializable]
    public class Biom
    {
       public ShopData Shop;

       [SerializeField] 
       public int Key;
       
       [SerializeField]
       public int LastPassedStageNumber;
       
       [SerializeField]
       public int CompletedStagesCount;

       [SerializeField]
       public int CurrentRoom;
    }
}