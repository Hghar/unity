using System.Linq;
using Realization.Configs;
using UnityEngine;

namespace Model.Inventories
{
    public class ItemBase : MonoBehaviour
    {
        [SerializeField] private ItemConfig[] _configs;

        public ItemConfig Find(string name)
        {
            ItemConfig config = _configs.FirstOrDefault((itemConfig => itemConfig.Name == name));
            if (config == default)
                return null;
            return config;
        }
    }
}