using System;
using UnityEngine;

namespace Realization.DevTools
{
    public class UnitSetup : MonoBehaviour
    {
        [SerializeField] private UnitPosition[] _units;
        
        private ConsoleDevTool _console;

        private void Start()
        {
            _console = FindObjectOfType<ConsoleDevTool>();
            foreach (var unit in _units)
            {
                _console.Perform($"s {unit.Position.x} {unit.Position.y} {unit.Uid}");
            }
        }
    }

    [Serializable]
    public struct UnitPosition
    {
        [SerializeField] public Vector2Int Position;
        [SerializeField] public string Uid;
    }
}