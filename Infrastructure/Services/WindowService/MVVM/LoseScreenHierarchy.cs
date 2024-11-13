using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class LoseScreenHierarchy : MonoBehaviour
    {
        public Button CloseClick;
        public CanvasGroup MainGroup;
        public Transform VictoryImage;
        public TMP_Text ContinueText;
        public TMP_Text HelpText;
        public Transform ItemsBackground;
    }
}