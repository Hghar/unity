using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sounds
{
    public class ButtonCountObserver : MonoBehaviour
    {
        [SerializeField] private Transform _observableContainer;
        [SerializeField] private ButtonClickSoundPlayer _soundPlayer;
        [SerializeField] private bool _isUpdatable;

        private int currentChildren = 0;

        private void Start()
        {
            IEnumerable buttons = _observableContainer.GetComponentsInChildren<Button>(true);
            _soundPlayer.SetNewButtons(buttons);
        }

        private void Update()
        {
            if (_isUpdatable)
            {
                if (_observableContainer.childCount != currentChildren)
                {
                    currentChildren = _observableContainer.childCount;
                    IEnumerable buttons = _observableContainer.GetComponentsInChildren<Button>(true);
                    _soundPlayer.SetNewButtons(buttons);
                }
            }
        }
    }
}