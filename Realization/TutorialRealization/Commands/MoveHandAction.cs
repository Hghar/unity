using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Extensions;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class MoveHandAction : IAction
    {
        private IObjectProvider<GameObject>[] _pivots;
        private TutorialHand _hand;

        public MoveHandAction(IObjectProvider<GameObject>[] pivots, TutorialHand hand)
        {
            _hand = hand;
            _pivots = pivots;
        }
        
        public async UniTask Perform()
        {
            GameObject target = await _pivots[0].GetAsync();
            RenderSpace type = target.RenderSpace();
            List<Transform> targets = new List<Transform>();
            foreach (var pivot in _pivots)
            {
                var gameObject = await pivot.GetAsync();
                targets.Add(gameObject.transform);
            }
            _hand.Follow(type, targets);
        }
    }
}