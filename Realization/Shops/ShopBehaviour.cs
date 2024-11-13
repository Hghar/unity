using Model.Economy;
using Realization.Economy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Realization.Shops
{
    public class ShopBehaviour : MonoBehaviour
    {
        public Button Reload;
        public Button Open;
        public Button Upgrade;
        public TMP_Text UpgradeStatus;
        public GameObject ShopPanel;
        public Transform EntitiesParent;
        public TMP_Text ReloadPrice;
        public TMP_Text UpgradePrice;
        public TMP_Text Grade1;
        public TMP_Text Grade2;
        public TMP_Text Grade3;
        public TMP_Text Grade4;
        public TMP_Text Grade5;
        public Image OpenStatus;
        public Image Gold;
        public Image Fade;
        public TMP_Text MinionCount;
        public TMP_Text MaxText;
        public Image MaxArrow;
        public Image Arrow;
        public Image Helm;
        public CounterAnimation CounterAnimation;

        public Transform CounterParent;
        
        public GradeAnimation GradeAnimation1;
        public GradeAnimation GradeAnimation2;
        public GradeAnimation GradeAnimation3;
        public GradeAnimation GradeAnimation4;
        public GradeAnimation GradeAnimation5;
        public ShopLevelAnimation ShopLevelAnimation;
        
        public ShopLevelUpButtonAnimation ArrowShopOpenButton;


        public Button SittingsButton;
    }
}