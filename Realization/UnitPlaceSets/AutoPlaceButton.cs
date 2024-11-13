using System;
using System.Linq;
using Battle;
using Fight.Fractions;
using Grids;
using Units;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Realization.UnitplaceSets
{
    public class AutoPlaceButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private MinionFactory _factory;
        private IBattleFinishPublisher _battleFinishPublisher;
        private IGrid<IMinion> _grid;

        [Inject]
        private void Construct(MinionFactory factory, IGrid<IMinion> grid, IBattleFinishPublisher battleFinishPublisher)
        {
            _battleFinishPublisher = battleFinishPublisher;
            _grid = grid;
            _factory = factory;
            _battleFinishPublisher.BattleFinished += () => _button.interactable = true;
        }

        private void Awake()
        {
            _button.onClick.AddListener(SetMinions);
        }

        private void SetMinions()
        {
            var set = new AutoPlace(_grid, false);
            set.PlaceMinions(_factory.Minions.Where((minion => minion.Fraction == Fraction.Minions)).ToArray());
        }
    }
}