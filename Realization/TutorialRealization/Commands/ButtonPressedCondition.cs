using System;
using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets.StepSheet.Commands.Conditions;
using UnityEngine;
using UnityEngine.UI;

namespace Realization.TutorialRealization.Commands
{
    public class ButtonPressedCondition : ICondition
    {
        private readonly IObjectProvider<Button> _button;
        private bool _inited;
        private bool _met;

        public ButtonPressedCondition(IObjectProvider<Button> button)
        {
            _button = button;
        }

        private void Meet()
        {
            _met = true;
            _button.Get().onClick.RemoveListener(Meet);
        }

        public async UniTask<bool> Met()
        {
            if (_inited == false)
            {
                try
                {
                    var button = await _button.GetAsync();
                    if (button == null)
                        return false;
                    
                    button.onClick.AddListener(Meet);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Can't find object {_button.Name}");
                    Debug.LogException(e);
                }

                _inited = true;
            }

            return _met;
        }
    }
}