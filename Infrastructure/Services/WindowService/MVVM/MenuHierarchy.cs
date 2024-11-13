using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class MenuHierarchy : MonoBehaviour
    {
        public TMP_Text StageText;
        public TMP_Text DungeonName;
        public TMP_Text GoldCounter;
        public TMP_Text CrystalsCounter;
        public Button RightClick;
        public Button LeftClick;
        public Button Sitting;
        public Button Collection;
        public Button Play;
        public Button Pumping; 
        public Image LockImage;
        public TMP_Text LockText;
        [FormerlySerializedAs("ItemsRoot")] public BiomItemsHierarchy _biomItemsRoot;
    }
}