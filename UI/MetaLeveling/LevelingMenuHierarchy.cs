using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Realization.States.CharacterSheet;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using Infrastructure.Services.StatsBoostService;
using Zenject;
using UnityEngine.Serialization;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class LevelingMenuHierarchy : MonoBehaviour
    {
        public LevelingCoins _levelingCoins;

        [Header("Configs")]
        public GeneralLevelingConfig _generalLevelingConfig;
        public ClassLevelingUpConfig _classLevelingUpConfig;
        public CharacterConfig _characterConfig;

        [Header("Leveling_Buttons")]
        public Button _allLevelingButton;
        public Button[] _classButtons;
        public Image[] _classButtonsNote;
        public TMP_Text[] _classButtonsText;

        [Header("Informations")]
        public TMP_Text _nameInformationText;
        public GameObject[] _paramInformationCases;

        public Information[] _paramGeneralInfo;
        public Information[] _paramClasslInfo;

        [Header("Close")]
        public Button _closeButton;

        [Header("Coins")]
        public TMP_Text _coinText;
        public TMP_Text _gemText;
        public TMP_Text _dublonText;

        [Header("Active_Buttons")]
        public Button _levelUpButton;
        public TMP_Text _levelUpText;
        public Button _resetButton;
        public TMP_Text _resetText;

        [Header("Effects")]
        public GameObject _cannotBeResetObjects;
        public Button _closeCannotBeReset;

        [FormerlySerializedAs("ItemsRoot")] public MetaLevelingHierarchy _metaLevelingItemsRoot;
    }
}

[Serializable]
public class Information
{
    public Information(string Name, string whiteText, string greenText)
    {
        NameText.text = Name;
        WhiteText.text = whiteText;
        GreenText.text = greenText;
    }

    public TMP_Text NameText;
    public TMP_Text WhiteText;
    public TMP_Text GreenText;
}
