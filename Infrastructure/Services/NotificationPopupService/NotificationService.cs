using System.Collections.Generic;
using System.Linq;
using Infrastructure.Helpers;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.WindowService;
using Infrastructure.Services.WindowService.MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Services.NotificationPopupService
{
    public class NotificationService : INotificationService
    {
        private const string RootPath = "WindowPrefabs/PopupRoot";
        private readonly IStaticDataService _staticDataService;
        private Dictionary<Alignment, Vector3> _alligments;
        private Dictionary<PopUpId, PopUpWindow> _windows = new ();
        private Sitting _defaultSittings =
                new Sitting(alignment: Alignment.Middle, id: PopUpId.Simple, withCloseButton: true);
        private Transform _uiRoot;

        public NotificationService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void CreateRoot()
        {
            GameObject gameObject = Resources.Load<GameObject>(RootPath);
            _uiRoot = Object.Instantiate(gameObject).transform;
            SceneObjectPool.Instance.Objects.Add(_uiRoot.gameObject);
            AlligmentHelper alligmentHelper = _uiRoot.GetComponent<AlligmentHelper>();
            _alligments = alligmentHelper.GetPoints().ToDictionary(x => x.Alignment, x => x.Transform.position);
        }

        public void SetDefaultSitting(Sitting sitting)
        {
            _defaultSittings = sitting;
        }

        public IPopup Show(string text, Sitting sitting)
        {
            PopUpWindowData data = _staticDataService.ForPopUpWindow(sitting.Id);
            PopUpWindow popup = Object.Instantiate(data.Prefab, _uiRoot);
            SceneObjectPool.Instance.Objects.Add(popup.gameObject);
            SceneObjectPool.Instance.Objects.Add(popup.GetComponentInChildren<Button>().gameObject);
            
            popup.Initialize(_alligments[sitting.Alignment],sitting.WithCloseButton,text);
            
            if (!sitting.WithCloseButton) 
                _windows.Add(sitting.Id,popup);

            return popup;
        }

        public IPopup Show(string text) => 
                Show(text,_defaultSittings);

        public void Close(PopUpId popUpId)
        {
            if(_windows[popUpId] == null)
            {
                _windows.Remove(popUpId);
                return;
            }
            
            _windows[popUpId].DestroyPopup();
            _windows.Remove(popUpId);
        }
        public class Sitting
        {
            public PopUpId Id;
            public Alignment Alignment;
            public bool WithCloseButton;

            public Sitting(Alignment alignment, PopUpId id, bool withCloseButton)
            {
                Alignment = alignment;
                Id = id;
                WithCloseButton = withCloseButton;
            }
        }
    }
}