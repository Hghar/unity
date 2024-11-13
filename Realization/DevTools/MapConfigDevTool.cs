using System;
using System.Reflection;
using Realization.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.DevTools
{
    public class MapConfigDevTool : MonoBehaviour
    {
        [SerializeField] private MapConfig _map;
        [SerializeField] private TMP_InputField _size;
        [SerializeField] private TMP_InputField _shops;
        [SerializeField] private TMP_InputField _minSegmentLength;
        [SerializeField] private TMP_InputField _chance;
        [SerializeField] private TMP_InputField _roomChange;
        [SerializeField] private Toggle _shopOnEmpty;

        private void Awake()
        {
            _size.onDeselect.AddListener(ChangeSize);
            _size.text = _map.MapSize.ToString();

            _shops.onDeselect.AddListener(ChangeShops);
            _shops.text = _map.ShopCount.ToString();

            _minSegmentLength.onDeselect.AddListener(ChangeMinSegmentLength);
            _minSegmentLength.text = _map.MinSegmentLength.ToString();

            _shopOnEmpty.onValueChanged.AddListener(OnShopOnEmptyChanged);
            _shopOnEmpty.isOn = _map.SpawnShopsOnEmpty;

            _chance.onValueChanged.AddListener(OnChanceChanged);
            _chance.text = _map.RotateChance.ToString();

            _roomChange.onValueChanged.AddListener(OnRoomChangeChanged);
            _roomChange.text = _map.ChangeRoomChance.ToString();
        }

        private void OnRoomChangeChanged(string arg0)
        {
            Type type = _map.GetType();
            PropertyInfo property = type.GetProperty(nameof(_map.ChangeRoomChance));

            if (float.TryParse(_roomChange.text, out float value))
            {
                property.SetValue(_map, value);
            }
        }

        private void OnChanceChanged(string arg0)
        {
            Type type = _map.GetType();
            PropertyInfo property = type.GetProperty(nameof(_map.RotateChance));

            if (float.TryParse(_chance.text, out float value))
            {
                property.SetValue(_map, value);
            }
        }

        public void ChangeSize(string args)
        {
            Type type = _map.GetType();
            PropertyInfo property = type.GetProperty(nameof(_map.MapSize));

            if (int.TryParse(_size.text, out int value))
            {
                property.SetValue(_map, value);
            }
        }

        private void ChangeShops(string args)
        {
            Type type = _map.GetType();
            PropertyInfo property = type.GetProperty(nameof(_map.ShopCount));

            if (int.TryParse(_shops.text, out int value))
            {
                property.SetValue(_map, value);
            }
        }

        private void ChangeMinSegmentLength(string args)
        {
            Type type = _map.GetType();
            PropertyInfo property = type.GetProperty(nameof(_map.MinSegmentLength));

            if (int.TryParse(_minSegmentLength.text, out int value))
            {
                property.SetValue(_map, value);
            }
        }

        private void OnShopOnEmptyChanged(bool args)
        {
            Type type = _map.GetType();
            PropertyInfo property = type.GetProperty(nameof(_map.SpawnShopsOnEmpty));
            property.SetValue(_map, args);
        }
    }
}