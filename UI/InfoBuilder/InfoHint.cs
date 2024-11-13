using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;
using System.Linq;
using AssetStore.HeroEditor.Common.CommonScripts;
using UnityEngine.UI;
using Parameters;
using Zenject;
using Realization.States.CharacterSheet;
using DG.Tweening;
using UnityEngine.EventSystems;
using UI;
using UnityEngine.Events;

public class InfoHint : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ParamsMenu _paramsMenu;
    [SerializeField] private PanelInformationInfoHint InfoPanel;
    [SerializeField] private Transform _pointer;
    [SerializeField] private CharacterConfig _characterConfig;

    private Dictionary<Button, UnityAction> _actions = new();

    private bool isNoUpdate = false;

    public List<CharacterInfoHint> Characters = new List<CharacterInfoHint>(Enum.GetNames(typeof(MinionClass)).Length);

    private List<CharacterOnSceneInformation> minionsOnScene = new List<CharacterOnSceneInformation>();

    private List<Vector3> startButtonPosition = new List<Vector3>();

    private Vector3 _startInfoPanelPosition = Vector3.zero;

    private List<CharacterSet> _characterSets = new List<CharacterSet>();

    public bool isActive => InfoPanel.gameObject.activeSelf;

    public event Action<MinionClass> clickButton;
    public event Action unclickButton;

    [Inject]
    private void Construct(CharacterSet[] characterSets)
    {
        _characterSets = new List<CharacterSet>(characterSets);
    }

    private void Start()
    {
        InfoPanel.gameObject.SetActive(false);

        _startInfoPanelPosition = InfoPanel.gameObject.transform.position;

        for (int i = 0; i< Characters.Count; i++)
        {
            MinionClass minionClass = Characters[i].Class;
            //Characters[i].Button.onClick.AddListener(() => ShowOnButton(minionClass));

            startButtonPosition.Add(Characters[i].Button.transform.position);
        }

        _characterSets = _characterConfig.CharacterSets;

        _characterSets.RemoveAll(x => x.UnitCount == 0);

        //_characterSets.Sort((s1, s2) => s1.Class.CompareTo(s2.Class));

        UpdateButtonPanel();
    }

    private void OnEnable()
    {
        StartCoroutine(TickInfoHintUpdate());
    }
    private void ShowInfo(MinionClass minionClass)
    {
        _paramsMenu.Hide();

        CharacterInfoHint character = Characters.Find(x => x.Class == minionClass);

        List<CharacterSet> characterSets = _characterSets.FindAll(x => x.Class == character.Class);

        string[] data = new List<string>(character.TextSkills).ToArray();

        int[] hintComplite = new int[characterSets.Count];

        character.TextSkills = new string[characterSets.Count];

        for(int i = 0; i < characterSets.Count; i++)
        {
            //if(i < data.Length)
            {
                //character.TextSkills[i] = data[i];
            }
            //else
            {
                character.TextSkills[i] = characterSets[i].TextEng;
            }

            hintComplite[i] = characterSets[i].UnitCount;
        }

        List<int> gradesActive = new List<int>();

        CharacterOnSceneInformation[] characterOnSceneInformation = minionsOnScene.FindAll(x => x.MinionClass == minionClass).ToArray();

        for (int i = 0; i < characterOnSceneInformation.Length; i++)
        {
            gradesActive.Add(characterOnSceneInformation[i].Grade);
        }

        InfoPanel.UpdateInformation(character.Class.ToString(), character.Class.ConvertToString() + "s", character.BonusName, character.CharacterImages, character.TextSkills, 
            gradesActive.ToArray(), hintComplite);

        InfoPanel.HintComplete(-1);

        int findIndex = 0;

        if(Characters.Find(x => x.Button.gameObject.GetComponent<Image>().sprite == character.GrayIcon) == null)
        {
            findIndex = Characters.FindIndex(x => x.Button.gameObject.GetComponent<Image>().sprite == character.Icon);

            UpdateTextInfoPanel(character);
        }
        else
        {
            findIndex = Characters.FindIndex(x => x.Button.gameObject.GetComponent<Image>().sprite == character.GrayIcon);
        }

        for (int i = 0; i < Characters.Count; i++)
        {
            if(i == findIndex)
            {
                continue;
            }

            if (Characters[i].Button.transform.localScale.x != 1)
            {
                Characters[i].Button.transform.DOScale(1f, 0.3f);
                Characters[i].Button.transform.GetChild(0).transform.DOScale(1f, 0.3f);
                Characters[i].Button.transform.GetChild(0).transform.DOMoveY(Characters[i].Button.transform.GetChild(0).transform.position.y - 0.5f, 0.3f);
                Characters[i].Button.transform.DOMoveY(Characters[i].Button.transform.position.y - 0.5f, 0.3f);
            }
        }

        Characters[findIndex].Button.transform.DOScale(1.1f, 0.3f);
        Characters[findIndex].Button.transform.GetChild(0).transform.DOScale(0.95f, 0.3f);
        Characters[findIndex].Button.transform.GetChild(0).transform.DOMoveY(Characters[findIndex].Button.transform.GetChild(0).transform.position.y + 0.5f, 0.3f);
        Characters[findIndex].Button.transform.DOMoveY(Characters[findIndex].Button.transform.position.y + 0.5f, 0.3f);

        if ((int)startButtonPosition[findIndex].y ==
            (int)startButtonPosition[0].y)
        {
            InfoPanel.gameObject.transform.position = _startInfoPanelPosition + Vector3.up * 95;
        }
        else
        {
            InfoPanel.gameObject.transform.position = _startInfoPanelPosition;
        }

        _pointer.position = new Vector3(
            startButtonPosition[findIndex].x,
            _pointer.position.y,
            _pointer.position.z);

        InfoPanel.gameObject.SetActive(true);
    }

    private bool UpdateTextInfoPanel(CharacterInfoHint character)
    {
        List<CharacterOnSceneInformation> charactersOnScene = minionsOnScene.FindAll(x => x.MinionClass == character.Class);
        List<CharacterSet> characterSets = _characterSets.FindAll(x => x.Class == character.Class);

        characterSets.RemoveAll(x => x.UnitCount == 0);

        charactersOnScene.Sort((s1, s2) => s1.Grade.CompareTo(s2.Grade));
        characterSets.Sort((s1, s2) => s1.UnitCount.CompareTo(s2.UnitCount));

        for (int i = 0; i < characterSets.Count; i++)
        {
            if(characterSets[i].UnitCount >= charactersOnScene.Count)
            {
                //if (i != 0)
                {
                    InfoPanel.HintComplete(i);

                    return true;
                }

                 //return false;
            }
        }

        return false;
    }

    private void CloseInfo()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            if (Characters[i].Button.transform.localScale.x != 1)
            {
                Characters[i].Button.transform.DOScale(1f, 0.3f);
                Characters[i].Button.transform.GetChild(0).transform.DOScale(1f, 0.3f);
                Characters[i].Button.transform.GetChild(0).transform.DOMoveY(Characters[i].Button.transform.GetChild(0).transform.position.y - 0.5f, 0.3f);
                Characters[i].Button.transform.DOMoveY(Characters[i].Button.transform.position.y - 0.5f, 0.3f);
            }
        }

        InfoPanel.gameObject.SetActive(false);
    }

    public void Bind(IUnit unit)
    {
        if (unit is IMinion minion)
        {
            //AddMininon(minion.Class);
        }
    }

    public void ShowOnMinion(IUnit unit)
    {
        if ((unit is IMinion minion))
        {
            //ShowInfo(minion.Class);
        }
    }

    public void CloseOnMinion()
    {
        //CloseInfo();
    }

    public void ShowOnButton(MinionClass minionClass)
    {
        ShowInfo(minionClass);

        clickButton.Invoke(minionClass);
    }

    public void CloseOnButton()
    {
        if(unclickButton != null)
            unclickButton.Invoke();

        CloseInfo();
    }
    public bool IsNewMinion(CharacterOnSceneInformation character)
    {
        var gradeSet = _characterSets.FindAll(x => (x.Class == character.MinionClass));

        var gradeSetArray = minionsOnScene.FindAll(x => (x.MinionClass == character.MinionClass));

        var gradeClone = minionsOnScene?.Find(x => (x.MinionClass == character.MinionClass && x.Grade == character.Grade));

        if (gradeClone == null)
        {
            for (int i = 0; i < gradeSet.Count; i++)
            {
                if(gradeSet[i].UnitCount < gradeSetArray.Count + 1)
                {
                    continue;
                }

                if (gradeSet[i].UnitCount == gradeSetArray.Count + 1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //�������
    public bool IsNewMinionOnScene(CharacterOnSceneInformation character)
    {
        if (minionsOnScene.Find(x => (x.MinionClass == character.MinionClass && x.Grade == character.Grade)) == null &&
            minionsOnScene.Count(x => x.MinionClass == character.MinionClass) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddMininon(CharacterOnSceneInformation character) 
    {
        if (minionsOnScene.Find(x => (x.MinionClass == character.MinionClass && x.Grade == character.Grade)) == null)
        {
            minionsOnScene.Add(character);

            minionsOnScene[minionsOnScene.Count - 1].cloneCount = 1;

            minionsOnScene.Sort((s1, s2) => s1.MinionClass.CompareTo(s2.MinionClass));

            UpdateButtonPanel();
        }
        else
        {
            minionsOnScene.Find(x => (x.MinionClass == character.MinionClass && x.Grade == character.Grade)).cloneCount++;
        }
    }

    public void RemoveMinion(CharacterOnSceneInformation character)
    {
        CharacterOnSceneInformation characterOn = minionsOnScene.Find(x => x.MinionClass == character.MinionClass && x.Grade == character.Grade);

        if (characterOn != null)
        {
            if (characterOn.cloneCount == 1)
            {
                characterOn.cloneCount = 0;

                InfoPanel.RemoveGrade(characterOn.Grade);

                minionsOnScene.Remove(characterOn);

                minionsOnScene = new List<CharacterOnSceneInformation>(minionsOnScene.ToArray());

                UpdateButtonPanel();
            }
            else if (characterOn.cloneCount > 1)
            {
                characterOn.cloneCount--;
            }
        }
    }

    public void Battle(bool isBattle)
    {
        isNoUpdate = isBattle;

        if(isNoUpdate == false)
        {
            UpdateButtonPanel();
        }
    }

    private IEnumerator TickInfoHintUpdate()
    {
        while (true)
        {
            if (isNoUpdate == false)
                if (minionsOnScene.Count > 0)
                    SetNumberUpdate();

            yield return new WaitForSeconds(0.5f);
        }
    }
    public void UpdateButtonPanel()
    {
        if(isNoUpdate == true)
        {
            return;
        }

        List<int> offButtonNumber = new List<int>();

        for(int i = 0; i < Enum.GetNames(typeof(MinionClass)).Length; i++)
        {
            if (minionsOnScene.Find(x => x.MinionClass == ((MinionClass)i)) != null)
            {
                offButtonNumber.Add(i);
            }
        }

        for(int i = 0; i< Characters.Count; i++)
        {
            if (_actions.ContainsKey(Characters[i].Button))
            {
                Characters[i].Button.onClick.RemoveListener(_actions[Characters[i].Button]);
                _actions.Remove(Characters[i].Button);
            }
            
            if (offButtonNumber.Count > i)
            {
                Characters[i].Button.gameObject.SetActive(true);

                List<CharacterSet> characterSets = _characterSets.FindAll(x => x.Class == Characters[offButtonNumber[i]].Class);
                characterSets.Sort((s1, s2) => s1.UnitCount.CompareTo(s2.UnitCount));

                if (minionsOnScene.FindAll(x => x.MinionClass == Characters[offButtonNumber[i]].Class).Count < 
                    characterSets[0].UnitCount)
                {
                    Characters[i].Button.gameObject.GetComponent<Image>().sprite = Characters[offButtonNumber[i]].GrayIcon;
                    Characters[i].Button.gameObject.GetComponent<Image>().overrideSprite = Characters[offButtonNumber[i]].GrayIcon;
                }
                else
                {
                    Characters[i].Button.gameObject.GetComponent<Image>().sprite = Characters[offButtonNumber[i]].Icon;
                    Characters[i].Button.gameObject.GetComponent<Image>().overrideSprite = Characters[offButtonNumber[i]].Icon;
                }

                MinionClass minionClass = (MinionClass)offButtonNumber[i];
                UnityAction onClick = () => ShowOnButton(minionClass);
                Characters[i].Button.onClick.AddListener(onClick);
                _actions.Add(Characters[i].Button, onClick);
            }
            else
            {
                Characters[i].Button.gameObject.SetActive(false);
            }
        }

        if(minionsOnScene.Count > 0)
            SetNumberUpdate();
    }

    private void SetNumberUpdate()
    {
        int set = 0;

        MinionClass findClass = minionsOnScene[0].MinionClass;

        CharacterInfoHint oldCharacter = Characters.Find(x => x.Button.gameObject.GetComponent<Image>().sprite == Characters[(int)findClass].GrayIcon);

        if (oldCharacter == null)
        {
            oldCharacter = Characters.Find(x => x.Button.gameObject.GetComponent<Image>().sprite == Characters[(int)findClass].Icon);
        }

        for (int i = 0; i < minionsOnScene.Count; i++)
        {
            findClass = minionsOnScene[i].MinionClass;

            CharacterInfoHint character = Characters.Find(x => x.Button.gameObject.GetComponent<Image>().sprite == Characters[(int)findClass].GrayIcon);

            if(character == null)
            {
                character = Characters.Find(x => x.Button.gameObject.GetComponent<Image>().sprite == Characters[(int)findClass].Icon);
            }

            InfoPanel.AddGrade(minionsOnScene[i].Grade);

            if (character.Class == oldCharacter.Class)
            {
                ++set;
            }
            
            if (set != 0 && i > 0)
            {
                int gradeNumber = 0;

                findClass = minionsOnScene[i - 1].MinionClass;

                var gradeSet = _characterSets.Find(x => (x.Class == findClass && x.UnitCount > set));
                if(gradeSet != null)
                    gradeNumber = gradeSet.UnitCount;
                
                if (gradeNumber == 0)
                {
                    gradeNumber = _characterSets.Find(x => (x.Class == findClass && x.UnitCount == set)).UnitCount;

                    gradeNumber = gradeNumber == 0 ? 999 : gradeNumber;
                }

                oldCharacter.Button.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = set + "/" + gradeNumber;

                if (character.Class != oldCharacter.Class)
                {
                    set = 1;

                    oldCharacter = character;
                }

                //set = 1;

            }
        }

        if (set != 0)
        {
            int gradeNumber = 0;

            findClass = minionsOnScene[minionsOnScene.Count - 1].MinionClass;

            try
            {
                gradeNumber = _characterSets.Find(x => (x.Class == findClass && x.UnitCount > set)).UnitCount;

            }
            catch
            {
                Debug.LogError("Grade Character is null");
                return;
            }

            if (gradeNumber == 0)
            {
                gradeNumber = _characterSets.Find(x => (x.Class == findClass && x.UnitCount == set)).UnitCount;

                gradeNumber = gradeNumber == 0 ? 999 : gradeNumber;
            }

            oldCharacter.Button.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = set + "/" + gradeNumber;

            set = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}

[Serializable]
public class CharacterInfoHint
{
    public MinionClass Class;
    public Button Button;
    public Button newButton;
    public Sprite Icon;
    public Sprite GrayIcon;
    public String Name;
    public String BonusName;
    public Sprite[] CharacterImages;
    public String[] TextSkills;
}

public class CharacterOnSceneInformation
{
    public MinionClass MinionClass;
    public int Grade;

    public int cloneCount;

    public CharacterOnSceneInformation(MinionClass minionClass, int grade)
    {
        MinionClass = minionClass;
        Grade = grade;
    }
}
