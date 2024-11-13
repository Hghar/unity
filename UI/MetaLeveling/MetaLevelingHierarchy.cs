using DG.Tweening;
using Infrastructure.Services.StatsBoostService;
using Realization.States.CharacterSheet;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MetaLevelingHierarchy : MonoBehaviour
{
    /*
    private int Level = 0;

    private IStatsBoostService _stats;

    public enum Leveling
    {
        General, Class, Warrior, Priest, Mage, Scout
    }

    [SerializeField] private Leveling _leveling = Leveling.General;

    [Inject]
    public void Construct(IStatsBoostService stats)
    {
        _stats = stats;
    }

    public void OnStart(Button resetButton)
    {
        resetButton.onClick.AddListener(() => { StartCoroutine(CannotBeReset()); });
    }

    private void OnEnable()
    {
        StartCoroutine(Initial());
    }

    private IEnumerator Initial()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        InitialGeneralInformation();
        InitialClassInformation();

        if (_stats != null)
        {
            InitialStats();
        }

        UpdateInfoNote();

        UpdateCoins();

        _levelUpButton.onClick.AddListener(() => { LevelUp(); UpdateCoins(); UpdateInfoNote(); });
    }

    private void UpdateInfoNote()
    {
        bool[] isPay;

        for (int i = 0; i < _classButtonsNote.Length; i++)
        {
            _classButtonsNote[i].enabled = true;

            if (i == 0)
            {
                isPay = _levelingCoins.IsPay(new PayCoins[1]
                    {
                    new PayCoins( PayCoins.idCoins.Golds, (int)_generalLevelingConfig.Configs[_levelingCoins.GlobalLevel + 1].GoldCost)
                    });
            }
            else
            {
                isPay = _levelingCoins.IsPay(new PayCoins[2]
                    {
                    new PayCoins( PayCoins.idCoins.Golds, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[i - 1] + 1].GoldCost),
                    new PayCoins( PayCoins.idCoins.Tokkens, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[i - 1] + 1].TokenCost)
                    });
            }

            for (int g = 0; g < isPay.Length; g++)
            {
                if (i == 0 && isPay[g] == false)
                {
                    _classButtonsNote[i].enabled = false;

                    break;
                }
                else if (isPay[g] == false)
                {
                    switch (i)
                    {
                        case 1:
                            for (int z = 0; z < 3; z++)
                                _classButtonsNote[4].enabled = false;
                            break;
                        case 2:
                            _classButtonsNote[1].enabled = false;
                            break;
                        case 3:
                            _classButtonsNote[3].enabled = false;
                            break;
                        case 4:
                            _classButtonsNote[2].enabled = false;
                            break;
                    }

                    if (i == 1)
                    {
                        _classButtonsNote[4].enabled = false;
                    }

                    break;
                }
            }
        }
    }

    private async void UpdateCoins()
    {
        await System.Threading.Tasks.Task.Delay(100);

        _coinText.text = _levelingCoins.Golds.ToString();
        _gemText.text = _levelingCoins.Gems.ToString();
        _dublonText.text = _levelingCoins.Tokkens.ToString();
    }

    private void InitialStats()
    {

    }
    private async void InitialGeneralInformation()
    {
        _allLevelingButton.onClick.AddListener(() => { UpdateGeneralInformation(_levelingCoins.GlobalLevel); _leveling = Leveling.General; });

        await System.Threading.Tasks.Task.Delay(100);

        UpdateGeneralInformation(_levelingCoins.GlobalLevel);
    }
    private void InitialClassInformation()
    {
        _classButtons[0].onClick.AddListener
            (() => { UpdateClassInformation(0, _levelingCoins.ClassLevel[1]); ClickEffect(1); _leveling = Leveling.Warrior; });
        _classButtons[1].onClick.AddListener
            (() => { UpdateClassInformation(1, _levelingCoins.ClassLevel[3]); ClickEffect(2); _leveling = Leveling.Priest; });
        _classButtons[2].onClick.AddListener
            (() => { UpdateClassInformation(2, _levelingCoins.ClassLevel[2]); ClickEffect(3); _leveling = Leveling.Mage; });
        _classButtons[3].onClick.AddListener
            (() => { UpdateClassInformation(3, _levelingCoins.ClassLevel[0]); ClickEffect(4); _leveling = Leveling.Scout; });

        _classButtonsText[1].text = "Lvl " + _levelingCoins.ClassLevel[1];
        _classButtonsText[2].text = "Lvl " + _levelingCoins.ClassLevel[3];
        _classButtonsText[3].text = "Lvl " + _levelingCoins.ClassLevel[2];
        _classButtonsText[4].text = "Lvl " + _levelingCoins.ClassLevel[0];
    }

    private void LevelUp()
    {
        switch (_leveling)
        {
            case Leveling.General:
                _levelingCoins.Pay(
                    new PayCoins[]
                    {
                        new PayCoins(PayCoins.idCoins.Golds, _generalLevelingConfig.Configs[_levelingCoins.GlobalLevel + 1].GoldCost) ,
                    });

                _levelingCoins.GeneralLevelUp();

                _allLevelingButton.onClick.Invoke();
                break;
            case Leveling.Warrior:
                _levelingCoins.Pay(
                    new PayCoins[2]
                        {
                            new PayCoins(PayCoins.idCoins.Golds, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[1]  + 1].GoldCost) ,
                            new PayCoins(PayCoins.idCoins.Tokkens, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[1] + 1].TokenCost)
                        });

                _levelingCoins.ClassLevelUp(LevelingCoins.idClass.Warrior);

                _classButtons[0].onClick.Invoke();
                break;
            case Leveling.Scout:
                _levelingCoins.Pay(
                    new PayCoins[2]
                        {
                            new PayCoins(PayCoins.idCoins.Golds, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[0] + 1].GoldCost) ,
                            new PayCoins(PayCoins.idCoins.Tokkens, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[0] + 1].TokenCost)
                        });

                _levelingCoins.ClassLevelUp(LevelingCoins.idClass.Scout);

                _classButtons[3].onClick.Invoke();
                break;
            case Leveling.Mage:
                _levelingCoins.Pay(
                    new PayCoins[2]
                        {
                            new PayCoins(PayCoins.idCoins.Golds, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[2] + 1].GoldCost) ,
                            new PayCoins(PayCoins.idCoins.Tokkens, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[2] + 1].TokenCost)
                        });

                _levelingCoins.ClassLevelUp(LevelingCoins.idClass.Mage);

                _classButtons[2].onClick.Invoke();
                break;
            case Leveling.Priest:
                _levelingCoins.Pay(
                    new PayCoins[2]
                        {
                            new PayCoins(PayCoins.idCoins.Golds, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[3] + 1].GoldCost) ,
                            new PayCoins(PayCoins.idCoins.Tokkens, (int)_classLevelingUpConfig.Configs[_levelingCoins.ClassLevel[3] + 1].TokenCost)
                        });

                _levelingCoins.ClassLevelUp(LevelingCoins.idClass.Priest);

                _classButtons[1].onClick.Invoke();
                break;
        }
    }

    private void ResetLevel()
    {
        _levelingCoins.Pay(
                    new PayCoins[1]
                        {
                            new PayCoins(PayCoins.idCoins.Gems, (int)_characterConfig.Constants.GeneralUpgradeResetCost)
                        });

        switch (_leveling)
        {
            case Leveling.Warrior:
                _levelingCoins.ClassLevelReset(LevelingCoins.idClass.Warrior);

                _classButtons[0].onClick.Invoke();
                break;
            case Leveling.Scout:
                _levelingCoins.ClassLevelReset(LevelingCoins.idClass.Scout);

                _classButtons[3].onClick.Invoke();
                break;
            case Leveling.Mage:
                _levelingCoins.ClassLevelReset(LevelingCoins.idClass.Mage);

                _classButtons[2].onClick.Invoke();
                break;
            case Leveling.Priest:
                _levelingCoins.ClassLevelReset(LevelingCoins.idClass.Priest);

                _classButtons[1].onClick.Invoke();
                break;
        }
    }

    private void UpdateGeneralInformation(int level)
    {
        ClickEffect(0);

        float[] stats = new float[7] { 0, 0, 0, 0, 0, 0, 0 };

        for (int g = 0; g < stats.Length; g++)
        {
            float newStat = 0;
            for (int i = 0; i <= level; i++)
            {
                switch (g)
                {
                    case 0:
                        newStat = _generalLevelingConfig.Configs[i].Health;
                        break;
                    case 1:
                        newStat = _generalLevelingConfig.Configs[i].Armor;
                        break;
                    case 2:
                        newStat = _generalLevelingConfig.Configs[i].Power;
                        break;
                    case 3:
                        newStat = _generalLevelingConfig.Configs[i].DodgeChance;
                        break;
                    case 4:
                        newStat = _generalLevelingConfig.Configs[i].CriticalDamageChance;
                        break;
                    case 5:
                        newStat = _generalLevelingConfig.Configs[i].CriticalDamageMultiplier;
                        break;
                    case 6:
                        newStat = _generalLevelingConfig.Configs[i].HealPower;
                        break;
                }

                stats[g] += newStat;
            }
        }

        _classButtonsText[0].text = "Lvl " + _levelingCoins.GlobalLevel;

        _nameInformationText.text = "Bonuses to all Heroes";

        _paramGeneralInformationText[0].transform.parent.gameObject.SetActive(true);
        _paramClassInformationText[0].transform.parent.parent.gameObject.SetActive(false);

        _paramGeneralInformationText[0].transform.GetChild(0).GetComponent<TMP_Text>().text = "Health";
        _paramGeneralInformationText[0].transform.GetChild(1).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && (_generalLevelingConfig.Configs[level + 1].Health) != 0
            ?
            stats[0].ToString() +
            ("<color=#44B901>(+" +
             (_generalLevelingConfig.Configs[level + 1].Health) + ")</color>") :
            stats[0].ToString();

        _paramGeneralInformationText[0].transform.GetChild(1).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[0].transform.GetChild(2).GetComponent<TMP_Text>().text = "Armor";
        _paramGeneralInformationText[0].transform.GetChild(3).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && (_generalLevelingConfig.Configs[level + 1].Armor) != 0
            ?
            stats[1].ToString() +
            ("<color=#44B901>(+" +
             (_generalLevelingConfig.Configs[level + 1].Armor) + ")</color>") :
            stats[1].ToString();

        _paramGeneralInformationText[0].transform.GetChild(3).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[1].transform.GetChild(0).GetComponent<TMP_Text>().text = "Power";
        _paramGeneralInformationText[1].transform.GetChild(1).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && (_generalLevelingConfig.Configs[level + 1].Power) != 0
            ?
            stats[2].ToString() +
            ("<color=#44B901>(+" +
             (_generalLevelingConfig.Configs[level + 1].Power) + ")</color>") :
            stats[2].ToString();

        _paramGeneralInformationText[1].transform.GetChild(1).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[1].transform.GetChild(2).GetComponent<TMP_Text>().text = "Chance of dodge";
        _paramGeneralInformationText[1].transform.GetChild(3).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && (_generalLevelingConfig.Configs[level + 1].DodgeChance) != 0
            ?
            stats[3].ToString() + "%" +
            ("<color=#44B901>(+" +
             _generalLevelingConfig.Configs[level + 1].DodgeChance + "%)</color>") :
            stats[3].ToString();

        _paramGeneralInformationText[1].transform.GetChild(3).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[2].transform.GetChild(0).GetComponent<TMP_Text>().text = "Critical damage chance";
        _paramGeneralInformationText[2].transform.GetChild(1).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && _generalLevelingConfig.Configs[level + 1].CriticalDamageChance != 0
            ?
            stats[4].ToString() + "%" +
            ("<color=#44B901>(+" +
             (_generalLevelingConfig.Configs[level + 1].CriticalDamageChance) + "%)</color>") :
            stats[4].ToString();

        _paramGeneralInformationText[2].transform.GetChild(1).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[2].transform.GetChild(2).GetComponent<TMP_Text>().text = "Critical damage mult";
        _paramGeneralInformationText[2].transform.GetChild(3).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && (_generalLevelingConfig.Configs[level + 1].CriticalDamageMultiplier) != 0
            ?
            stats[5].ToString() + "%" +
            ("<color=#44B901>(+" +
             (_generalLevelingConfig.Configs[level + 1].CriticalDamageMultiplier) + "%)</color>") :
            stats[5].ToString();

        _paramGeneralInformationText[2].transform.GetChild(3).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[3].transform.GetChild(0).GetComponent<TMP_Text>().text = "Power of healing";
        _paramGeneralInformationText[3].transform.GetChild(1).GetComponent<TMP_Text>().text = (level + 1) <= _generalLevelingConfig.Configs.Count - 1
            && (_generalLevelingConfig.Configs[level + 1].HealPower - _generalLevelingConfig.Configs[level].HealPower) != 0
            ?
            stats[6].ToString() +
            ("<color=#44B901>(+" +
             (_generalLevelingConfig.Configs[level + 1].HealPower) + ")</color>") :
            stats[6].ToString();

        _paramGeneralInformationText[3].transform.GetChild(1).DOPunchScale(Vector3.one * 0.2f, 1f, 0);

        _paramGeneralInformationText[3].transform.GetChild(2).GetComponent<TMP_Text>().text = "";
        _paramGeneralInformationText[3].transform.GetChild(3).GetComponent<TMP_Text>().text = "";

        if ((level + 1) <= _generalLevelingConfig.Configs.Count - 1)
            UpdateActionButtons(Leveling.General, 0, level, false);
        else
            UpdateActionButtons(Leveling.General, 0, level, true);
    }
    public void UpdateClassInformation(int id, int level)
    {
        string name = "Bonuses to All ";

        bool isNextLevel = false;

        float[] stats = new float[3] { 0, 0, 0 };

        _paramGeneralInformationText[0].transform.parent.gameObject.SetActive(false);
        _paramClassInformationText[0].transform.parent.parent.gameObject.SetActive(true);

        if (level + 1 <= _classLevelingUpConfig.Configs.Count - 1)
        {
            isNextLevel = true;
        }

        for (int g = 0; g < stats.Length; g++)
        {
            for (int i = 0; i <= level; i++)
            {
                float newStat = 0;

                switch (g)
                {
                    case 0:
                        newStat = _classLevelingUpConfig.Configs[i].Stats.List[id].Health;
                        break;
                    case 1:
                        newStat = _classLevelingUpConfig.Configs[i].Stats.List[id].Power;
                        break;
                    case 2:
                        newStat = _classLevelingUpConfig.Configs[i].Stats.List[id].Armor;
                        break;
                }

                stats[g] += newStat;
            }
        }

        switch (id)
        {
            case 0:
                name += "warriors";

                _classButtonsText[id + 1].text = "Lvl " + _levelingCoins.ClassLevel[1];
                break;
            case 1:
                name += "priests";

                _classButtonsText[id + 1].text = "Lvl " + _levelingCoins.ClassLevel[3];
                break;
            case 2:
                name += "mages";

                _classButtonsText[id + 1].text = "Lvl " + _levelingCoins.ClassLevel[2];
                break;
            case 3:
                name += "scouts";

                _classButtonsText[id + 1].text = "Lvl " + _levelingCoins.ClassLevel[0];
                break;
        }

        _nameInformationText.text = name;

        if (isNextLevel)
        {
            _paramClassInformationText[0].text = "Health  " + stats[0] + "%" +
                ((_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Health) != 0 ? (
                "<color=#44B901>(+"
                + (_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Health)
                + "%)</color>") : "");

            if (_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Power != 0)
                _paramClassInformationText[0].gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 0);

            _paramClassInformationText[1].text = "Power  " + stats[1] + "%" +
                (_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Power != 0 ?
                ("<color=#44B901>(+"
                + (_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Power)
                + "%)</color>") : "");

            if ((_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Power) != 0)
                _paramClassInformationText[1].gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 0);

            _paramClassInformationText[2].text = "Armor  " + stats[2] + "%" +
                (_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Armor != 0 ?
                ("<color=#44B901>(+"
                + (_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Armor)
                + "%)</color>") : "");

            if ((_classLevelingUpConfig.Configs[level + 1].Stats.List[id].Armor) != 0)
                _paramClassInformationText[2].gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 0);

            UpdateActionButtons(Leveling.Class, id, level, false);
        }
        else
        {
            _paramClassInformationText[0].text = "Health  " + stats[0] + "%";
            _paramClassInformationText[0].gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 0);

            _paramClassInformationText[1].text = "Power  " + stats[1] + "%";
            _paramClassInformationText[1].gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 0);

            _paramClassInformationText[2].text = "Armor  " + stats[2] + "%";
            _paramClassInformationText[2].gameObject.transform.DOPunchScale(Vector3.one * 0.2f, 1f, 0);

            UpdateActionButtons(Leveling.Class, id, level, true);
        }
    }

    public void UpdateActionButtons(Leveling leveling, int id, int level, bool isMaxLevel)
    {
        bool[] isPay;

        switch (leveling)
        {
            case Leveling.General:
                _resetButton.gameObject.SetActive(false);

                if (level + 1 == _generalLevelingConfig.Configs.Count)
                    isMaxLevel = true;

                isPay = _levelingCoins.IsPay(new PayCoins[2]
                {
                    new PayCoins( PayCoins.idCoins.Golds, (int)_generalLevelingConfig.Configs[isMaxLevel ? 0 : level + 1].GoldCost),
                    new PayCoins( PayCoins.idCoins.Gems, (int)_characterConfig.Constants.GeneralUpgradeResetCost)
                });

                LevelUpButtonCost(Leveling.General, isPay[0], isPay[1], false, level, isMaxLevel);
                break;
            case Leveling.Class:
                _resetButton.gameObject.SetActive(true);

                if (level + 1 == _classLevelingUpConfig.Configs.Count)
                    isMaxLevel = true;

                isPay = _levelingCoins.IsPay(new PayCoins[3]
                {
                    new PayCoins( PayCoins.idCoins.Golds, (int)_classLevelingUpConfig.Configs[isMaxLevel ? 0 : level + 1].GoldCost),
                    new PayCoins( PayCoins.idCoins.Tokkens, (int)_classLevelingUpConfig.Configs[isMaxLevel ? 0 : level + 1].TokenCost),
                    new PayCoins( PayCoins.idCoins.Gems, (int)_characterConfig.Constants.GeneralUpgradeResetCost)
                });

                LevelUpButtonCost(Leveling.Class, isPay[0], isPay[2], isPay[1], level, isMaxLevel);

                break;
        }
    }

    public void LevelUpButtonCost(Leveling leveling, bool isGold, bool isGems, bool isTokken, int level, bool isMaxLevel = false)
    {
        switch (leveling)
        {
            case Leveling.General:
                if (!isGold)
                {
                    _levelUpText.text = "Lvl Up \n <sprite name=\"Gold\"> <color=#EB4153>x" + _generalLevelingConfig.Configs[level + 1].GoldCost + "</color>";

                    _levelUpButton.interactable = false;
                }
                else
                {
                    _levelUpText.text = "Lvl Up \n <sprite name=\"Gold\"> <color=#FFFFFF>x" + _generalLevelingConfig.Configs[level + 1].GoldCost + "</color>";

                    _levelUpButton.interactable = true;
                }

                if (isMaxLevel)
                {
                    _levelUpText.text = "Max Lvl";

                    _levelUpButton.interactable = false;
                }
                break;
            case Leveling.Class:
                if (!isGold)
                {
                    _levelUpText.text = "Lvl Up \n <sprite name=\"Gold\"> <color=#EB4153>x" + _classLevelingUpConfig.Configs[level + 1].GoldCost + "</color>";

                    _levelUpButton.interactable = false;
                }
                else
                {
                    _levelUpText.text = "Lvl Up \n <sprite name=\"Gold\"> <color=#FFFFFF>x" + _classLevelingUpConfig.Configs[level + 1].GoldCost + "</color>";

                    _levelUpButton.interactable = true;
                }

                if (!isTokken)
                {
                    _levelUpText.text += "<sprite name=\"Galeon\"> <color=#EB4153>x" + _classLevelingUpConfig.Configs[level + 1].TokenCost + "</color>";

                    _levelUpButton.interactable = false;
                }
                else
                {
                    _levelUpText.text += "<sprite name=\"Galeon\"> <color=#FFFFFF>x" + _classLevelingUpConfig.Configs[level + 1].TokenCost + "</color>";
                }

                if (isMaxLevel)
                {
                    _levelUpText.text = "Max Lvl";

                    _levelUpButton.interactable = false;
                }

                if (!isGems)
                {
                    _resetText.text = "Reset \n <sprite name=\"Gem\"> <color=#EB4153>x" + _characterConfig.Constants.GeneralUpgradeResetCost + "</color>";

                    _resetButton.interactable = false;
                }
                else
                {
                    _resetText.text = "Reset \n <sprite name=\"Gem\"> <color=#FFFFFF>x" + _characterConfig.Constants.GeneralUpgradeResetCost + "</color>";

                    _resetButton.interactable = true;
                }
                break;
        }
    }

    private void ClickEffect(int buttonId)
    {
        if (buttonId == 0)
        {
            _allLevelingButton.transform.GetChild(0).gameObject.SetActive(true);
            for (int i = 0; i < _classButtons.Length; i++)
            {
                _classButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else
        {
            _allLevelingButton.transform.GetChild(0).gameObject.SetActive(false);

            for (int i = 0; i < _classButtons.Length; i++)
            {
                if (i != buttonId - 1)
                    _classButtons[i].transform.GetChild(0).gameObject.SetActive(false);
                else
                    _classButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            }

        }
    }
    private IEnumerator CannotBeReset()
    {
        int level = -1;

        switch (_leveling)
        {
            case Leveling.Warrior:
                level = _levelingCoins.ClassLevel[1];

                _classButtonsText[1].text = "Lvl " + _levelingCoins.ClassLevel[1];
                break;
            case Leveling.Priest:
                level = _levelingCoins.ClassLevel[3];

                _classButtonsText[2].text = "Lvl " + _levelingCoins.ClassLevel[3];
                break;
            case Leveling.Mage:
                level = _levelingCoins.ClassLevel[2];

                _classButtonsText[3].text = "Lvl " + _levelingCoins.ClassLevel[2];
                break;
            case Leveling.Scout:
                level = _levelingCoins.ClassLevel[0];

                _classButtonsText[4].text = "Lvl " + _levelingCoins.ClassLevel[0];
                break;
        }

        if (level == 0)
        {
            _cannotBeResetObjects.GetComponent<Image>().color = Color.clear;
            _cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.clear;

            _cannotBeResetObjects.SetActive(true);

            _cannotBeResetObjects.GetComponent<Image>().DOColor(Color.black, 0.2f);
            _cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.white, 0.2f);

            yield return new WaitForSeconds(1f);

            _cannotBeResetObjects.GetComponent<Image>().DOColor(Color.clear, 0.2f);
            _cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.clear, 0.2f);

            yield return new WaitForSeconds(0.3f);

            _cannotBeResetObjects.SetActive(false);
        }
        else if (level != -1)
        {
            ResetLevel();
            UpdateCoins();
            UpdateInfoNote();
        }
    }
    */
}
