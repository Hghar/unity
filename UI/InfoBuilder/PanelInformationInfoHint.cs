using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelInformationInfoHint : MonoBehaviour
{
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text BonusName;
    [SerializeField] private Image[] CharacterImages;
    [SerializeField] private TMP_Text[] TextSkills;

    [SerializeField] private Color32[] _newColors;

    private void Start()
    {
        Color32 newColor = new Color32(142, 142, 142, 100);

        for (int i = 0; i < CharacterImages.Length; i++)
        {
            newColor = new Color32(_newColors[i].r, _newColors[i].g, _newColors[i].b, newColor.a);
            CharacterImages[i].transform.parent.GetComponent<Image>().color = newColor;
            CharacterImages[i].color = new Color32(255, 255, 255, 150);
        }
    }

    public void UpdateInformation(string minionClassName, string name, string bonusName, Sprite[] characterImages, string[] textSkills, int[] grades, int[] hintComplites)
    {
        //Icon.sprite = icon;
        Name.text = "<sprite name=\"" + minionClassName + "\">" + name;
        BonusName.text = bonusName;

        Color32 newColor = new Color32(142, 142, 142, 100);

        for (int i = 0; i < CharacterImages.Length; i++)
        {
            newColor = new Color32(_newColors[i].r, _newColors[i].g, _newColors[i].b, newColor.a);
            CharacterImages[i].transform.parent.GetComponent<Image>().color = newColor;
            CharacterImages[i].color = new Color32(255, 255, 255, 150);
        }

        for (int i = 0; i < CharacterImages.Length; i++)
        {
            if (grades.Length > i)
            {
                AddGrade(grades[i]);
            }
        }

        for (int i = 0; i < CharacterImages.Length; i++)
        {
            if(characterImages.Length > i)
            {
                CharacterImages[i].transform.parent.transform.gameObject.SetActive(true);

                CharacterImages[i].sprite = characterImages[i];
            }
            else
            {
                CharacterImages[i].transform.parent.transform.gameObject.SetActive(false);
            }
        }

        for(int i = 0; i < TextSkills.Length; i++)
        {
            if (textSkills.Length > i)
            {
                TextSkills[i].transform.parent.transform.gameObject.SetActive(true);

                TextSkills[i].text = textSkills[i];

                TextSkills[i].transform.parent.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = hintComplites[i].ToString();
            }
            else
            {
                TextSkills[i].transform.parent.transform.gameObject.SetActive(false);
            }
        }
    }

    public void HintComplete(int gradeNumber)
    {
        for(int i = 0; i < TextSkills.Length; i++)
        {
            if(i != gradeNumber)
            {
                TextSkills[i].color = new Color32(255, 255, 255, 155);
            }
            else
            {
                TextSkills[i].color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public void AddGrade(int grade)
    {
        --grade;
        Color32 newColor = new Color32(142, 142, 142, 100);

        if (grade < _newColors.Length)
        {
            newColor = _newColors[grade];
        }

        CharacterImages[grade].transform.parent.GetComponent<Image>().color = newColor;

        if(newColor.a > 100)
            CharacterImages[grade].color = new Color32(255, 255, 255, 255);

        //Debug.LogError(CharacterImages[grade].transform.parent.GetComponent<Image>().color + " / " + _newColors[grade]);
    }

    public void RemoveGrade(int grade)
    {
        try
        {
            --grade;
            Color32 newColor = new Color32(142, 142, 142, 100);

            if (grade < _newColors.Length)
            {
                newColor = new Color32(_newColors[grade].r, _newColors[grade].g, _newColors[grade].b, newColor.a);

                if (CharacterImages[grade] != null)
                {
                    CharacterImages[grade].transform.parent.GetComponent<Image>().color = newColor;
                    CharacterImages[grade].color = new Color32(255, 255, 255, 150);
                }
            }
        }
        catch (Exception e)
        {
           Debug.LogException(e);
        }
    }
}
