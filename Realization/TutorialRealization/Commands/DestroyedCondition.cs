using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Conditions;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class DestroyedCondition : ICondition
    {
        private readonly IObjectProvider<GameObject> _destroyed;

        public DestroyedCondition(IObjectProvider<GameObject> destroyed)
        {
            _destroyed = destroyed;
        }

        public UniTask<bool> Met() => new(_destroyed.Get() == null);
    }
}