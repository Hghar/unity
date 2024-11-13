using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Entities.ShopItems
{
    [CreateAssetMenu(fileName = "Icons", menuName = "Configs/Icons", order = 0)]
    public class Icons : ScriptableObject
    {
        public IconPair[] _classes;
        public IconPair[] _mains;
        public List<Sprite> _frames;

        public IconsProvider Get()
        {
            return new IconsProvider(
                ToDictionary(_classes), 
                MainsToDictionary(_mains),
                _frames);
        }

        private Dictionary<KeyValuePair<MinionClass,int>,Sprite> MainsToDictionary(IconPair[] pairs)
        {
            var dictionary = new Dictionary<KeyValuePair<MinionClass,int>,Sprite>();
            foreach (var pair in pairs)
            {
                dictionary.Add(new KeyValuePair<MinionClass, int>(pair.Class, pair.Grade), pair.Sprite);
            }
            return dictionary;
        }

        private Dictionary<MinionClass, Sprite> ToDictionary(IconPair[] pairs)
        {
            var dictionary = new Dictionary<MinionClass, Sprite>();
            foreach (var pair in pairs)
            {
                dictionary.Add(pair.Class, pair.Sprite);
            }
            return dictionary;
        }
    }

    [Serializable]
    public struct IconPair
    {
        public MinionClass Class;
        public int Grade;
        public Sprite Sprite;
    }
}