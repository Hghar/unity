using System;
using NaughtyAttributes;
using UnityEngine;

namespace Realization.States
{
    [Serializable]
    public struct State
    {
        [ReadOnly] [SerializeField] private string _name;

        public State(string name)
        {
            _name = name;
        }

        public string Name => _name;
    }
}