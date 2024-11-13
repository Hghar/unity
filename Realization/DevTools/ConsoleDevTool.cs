using System;
using System.Collections.Generic;
using System.Linq;
using Fight.Damaging;
using Fight.Fractions;
using Grids;
using Realization.States.CharacterSheet;
using TMPro;
using Units;
using UnityEngine;
using Zenject;

namespace Realization.DevTools
{
    public class ConsoleDevTool : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private EffectInvoker _effectInvoker;

        private MinionFactory _factory;
        private CharacterConfig _characterConfig;
        private IGrid<IMinion> _grid;
        private readonly List<IMinion> _minions = new();

        [Inject]
        private void Construct(MinionFactory factory, CharacterConfig characterConfig, IGrid<IMinion> grid)
        {
            _factory = factory;
            _characterConfig = characterConfig;
            _grid = grid;
        }

        private void Awake()
        {
            _input.onEndEdit.AddListener(ReadCommand);
        }

        private void ReadCommand(string input)
        {
            var args = input.Split(' ');
            try
            {
                int x;
                int y;
                switch (args[0])
                {
                    case "d":
                        int.TryParse(args[1], out x);
                        int.TryParse(args[2], out y);
                        _minions
                            .Find((minion => minion.Position == new Vector2Int(x - 1, y - 1)))
                            .Damage(new Damage(float.MaxValue));
                        break;
                    case "s":
                        int.TryParse(args[1], out x);
                        int.TryParse(args[2], out y);
                        IMinion unit;
                        if (int.TryParse(args[3], out var indexInSheet))
                        {
                            unit = _factory.CreateAndReturn(_characterConfig.Characters[indexInSheet]);
                        }
                        else
                        {
                            Debug.Log($"Spawning {args[3]}");
                            var config = _characterConfig.Characters.First((character => character.Uid == args[3]));

                            if (args.Length > 4)
                            {
                                config.Level = Convert.ToInt32(args[4]);
                            }

                            unit = _factory.CreateAndReturn(config);
                        }

                        _minions.Add(unit);
                        _grid.Place(unit, x - 1, y - 1);

                        break;
                    case "c":
                        _effectInvoker.DoCommand(args[1]);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Perform(string command)
        {
            ReadCommand(command);
        }
    }
}
