using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Realization.Configs
{
    [CreateAssetMenu(fileName = "StartItemsConfig", menuName = "Configs/Start Items", order = 0)]
    public class StartItemsConfig : ScriptableObject
    {
        [SerializeField] private List<ItemConfig> _items;

        public IEnumerable Items => _items;
    }
}