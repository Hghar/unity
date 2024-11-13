using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Realization.TutorialRealization.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class HandAction : IAction
    {
        private TutorialHand _hand;
        private IObjectProvider<GameObject> _target;
        private bool _flip;
        private float _rotation;
        private Vector2 _offset;

        public HandAction(TutorialHand hand, DelayedObject target, bool flip, float rotation, Vector2 vector2)
        {
            _offset = vector2;
            _rotation = rotation;
            _flip = flip;
            _hand = hand;
            _target = target;
        }

        public async UniTask Perform()
        {
            GameObject target = await _target.GetAsync();
            RenderSpace type = target.RenderSpace();
            _hand.Follow(type, target.transform, _flip, _rotation, _offset);
        }
    }
}