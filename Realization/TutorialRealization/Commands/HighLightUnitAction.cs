using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Actions;
using Units;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class HighLightUnitAction : IAction
    {
        private IObjectProvider<GameObject> _unit;
        private bool _highlight;

        public HighLightUnitAction(IObjectProvider<GameObject> unit, bool highlight)
        {
            _highlight = highlight;
            _unit = unit;
        }
        
        public async UniTask Perform()
        {
            GameObject gameObject = await _unit.GetAsync();
            var unit = gameObject.GetComponent<IMinion>();
                
            Transform[] children = unit.CharacterParent.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if(_highlight)
                    child.gameObject.layer = LayerMask.NameToLayer("Clickable");
                else
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}