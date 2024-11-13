using System;
using Infrastructure.Services.NotificationPopupService;
using UnityEngine;

namespace Infrastructure.Services.WindowService
{
    [Serializable]
    public class WindowData
    {
        public WindowId WindowId;
        public GameObject Prefab;
    }
    
    [Serializable]
    public class PopUpWindowData
    {
        public PopUpId Id;
        public PopUpWindow Prefab;
    }
}