using Infrastructure.Services.StaticData;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.Services.WindowService
{
    public class OpenWindowButton:MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private WindowId _windowId;
        private IWindowService _windowService;

        private void Awake()
        {
            _button.onClick.AddListener(OpenWindow);
        }

        [Inject]
        public void Construct(IWindowService windowService)
        {
            _windowService = windowService;
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OpenWindow);
        }

        private void OpenWindow()
        {
            _windowService.Open(_windowId);
        }
    }
}