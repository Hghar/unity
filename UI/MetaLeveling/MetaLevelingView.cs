using DG.Tweening;
using Infrastructure.Services.StaticData;
using Infrastructure.Services.StatsBoostService;
using Infrastructure.Services.WindowService.MVVM;
using Infrastructure.Services.WindowService.ViewFactory;
using Parameters;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class MetaLevelingView : View<LevelingMenuHierarchy, LevelingViewModel>
{
    private bool _isAnimation = false;
    public MetaLevelingView(LevelingMenuHierarchy hierarchy, IViewFactory viewFactory,
            IStaticDataService staticDataService, IStatsBoostService statsBoostService) : base(hierarchy, viewFactory)
    {

        //Hierarchy._nameInformationText.text = "";

        Hierarchy._resetButton.gameObject.SetActive(false);
    }

    protected override void UpdateViewModel(LevelingViewModel viewModel)
    {
        _isAnimation = viewModel.IsAnimation;

        UpdateInfo(viewModel);
    }

    public void UpdateInfo(LevelingViewModel viewModel, bool isAnimation = false)
    {
        var select = viewModel.ReternSelectMenu();

        _isAnimation = isAnimation;

        UpdateNote(select);

        UpdateText(select);

        UpdateButtons(select);

        UpdateLight(select._class);

        UpdateLevelNumber(select);
    }

    private void UpdateText(MetaLevelingInfo select)
    {
        Hierarchy._nameInformationText.text = select.HeadingText;

        if (select._class == Class.General)
        {
            Hierarchy._paramInformationCases[0].SetActive(true);
            Hierarchy._paramInformationCases[1].SetActive(false);

            for (int i = 0; i < Hierarchy._paramGeneralInfo.Length - 1; i++)
            {
                UpdateOneTextStatGeneral(i, select);
            }
        }
        else
        {
            Hierarchy._paramInformationCases[0].SetActive(false);
            Hierarchy._paramInformationCases[1].SetActive(true);

            for (int i = 0; i < Hierarchy._paramClasslInfo.Length; i++)
            {
                UpdateOneTextStatMinion(i, select);
            }
        }
    }

    private void UpdateOneTextStatGeneral(int i, MetaLevelingInfo select)
    {
        Hierarchy._paramGeneralInfo[i].WhiteText.gameObject.transform.DOKill();
        Hierarchy._paramGeneralInfo[i].GreenText.gameObject.transform.DOKill();

        Hierarchy._paramGeneralInfo[i].WhiteText.gameObject.transform.localScale = Vector3.one;

        Hierarchy._paramGeneralInfo[i].NameText.text = select.GeneralInformation[i].NameText;

        Sequence sequenceWhite = DOTween.Sequence();
        Sequence sequenceGreen = DOTween.Sequence();

        string textGreen = "";

        if (select.GeneralInformation[i].GreenText != "0%" && select.GeneralInformation[i].GreenText != "0")
        {
            textGreen = $"<color=#44B901>(+{select.GeneralInformation[i].GreenText})</color>";
        }
        else
        {
            textGreen = "";
        }


        if (select.GeneralInformation[i].isUpdate && _isAnimation)
            sequenceWhite.Append(Hierarchy._paramGeneralInfo[i].WhiteText.gameObject.transform.DOScale(Vector3.one * 1.7f, 0.5f));

        sequenceWhite.AppendCallback(() => Hierarchy._paramGeneralInfo[i].WhiteText.text = select.GeneralInformation[i].WhiteText);

        if (select.GeneralInformation[i].isUpdate && _isAnimation)
            sequenceWhite.Append(Hierarchy._paramGeneralInfo[i].WhiteText.gameObject.transform.DOScale(Vector3.one, 0.5f));

        if (_isAnimation)
            sequenceGreen.Append(Hierarchy._paramGeneralInfo[i].GreenText.DOFade(0, 0.5f));

        sequenceGreen.AppendCallback(() => Hierarchy._paramGeneralInfo[i].GreenText.text = textGreen);

        if (_isAnimation)
            sequenceGreen.Append(Hierarchy._paramGeneralInfo[i].GreenText.DOFade(1, 0.5f));
    }
    private void UpdateOneTextStatMinion(int i, MetaLevelingInfo select)
    {
        Hierarchy._paramClasslInfo[i].WhiteText.gameObject.transform.localScale = Vector3.one;

        Hierarchy._paramClasslInfo[i].NameText.text = select.ClassInformation[i].NameText;

        Sequence sequenceWhite = DOTween.Sequence();
        Sequence sequenceGreen = DOTween.Sequence();

        string textGreen = "";

        if (select.ClassInformation[i].GreenText != "" || select.ClassInformation[i].GreenText != null)
        {
            textGreen = $"<color=#44B901>(+{select.ClassInformation[i].GreenText})</color>";
        }
        else
        {
            textGreen = "";
        }


        if (select.ClassInformation[i].isUpdate && _isAnimation)
            sequenceWhite.Append(Hierarchy._paramClasslInfo[i].WhiteText.gameObject.transform.DOScale(Vector3.one * 1.7f, 0.5f));

        sequenceWhite.AppendCallback(() => Hierarchy._paramClasslInfo[i].WhiteText.text = select.ClassInformation[i].WhiteText);

        if (select.ClassInformation[i].isUpdate && _isAnimation)
            sequenceWhite.Append(Hierarchy._paramClasslInfo[i].WhiteText.gameObject.transform.DOScale(Vector3.one, 0.5f));

        if (_isAnimation)
            sequenceGreen.Append(Hierarchy._paramClasslInfo[i].GreenText.DOFade(0, 0.5f));

        sequenceGreen.AppendCallback(() => Hierarchy._paramClasslInfo[i].GreenText.text = textGreen);

        if (_isAnimation)
            sequenceGreen.Append(Hierarchy._paramClasslInfo[i].GreenText.DOFade(1, 0.5f));

    }

    private void UpdateLight(Class @class)
    {
        for(int i = 0; i < Hierarchy._classButtons.Length + 1; i++)
        {
            if(i == (int)@class)
            {
                if (i == 0)
                    Hierarchy._allLevelingButton.transform.GetChild(0).gameObject.SetActive(true);
                else
                    Hierarchy._classButtons[i - 1].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                if (i == 0)
                    Hierarchy._allLevelingButton.transform.GetChild(0).gameObject.SetActive(false);
                else
                    Hierarchy._classButtons[i - 1].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void UpdateNote(MetaLevelingInfo data)
    {
        for(int i = 0; i < data.GoldCost.Length; i++)
        {
            if (i == 0)
            {
                Hierarchy._classButtonsNote[i].enabled = (data.GoldCost[i].IsBay && data.levels[i] < 9);
            }
            else
            {
                Hierarchy._classButtonsNote[i].enabled = (data.GoldCost[i].IsBay && data.TokkenCost[i].IsBay && data.levels[i] < 9);
            }
        }
    }

    private void UpdateButtons(MetaLevelingInfo data)
    {
        Hierarchy._resetText.text = "Reset\n";
        Hierarchy._levelUpText.text = "Lvl up\n";

        if (data.IsBayReset)
        {
            Hierarchy._resetButton.interactable = true;
            Hierarchy._resetText.text += "<sprite name=\"Gem\"> x " + data.CrystalCost.ToString();
        }
        else
        {
            Hierarchy._resetButton.interactable = false;
            Hierarchy._resetText.text += "<sprite name=\"Gem\"><color=#EB4154> x " + data.CrystalCost.ToString() + "</color>";
        }
        
        if(data._class == Class.General)
        {
            if (data.GoldCost[0].IsBay && data.levels[0] < 9)
            {
                Hierarchy._levelUpButton.interactable = true;
                Hierarchy._levelUpText.text +=
                    "<sprite name=\"Gold\"> x " + data.GoldCost[0].Value.ToString();
            }
            else if (data.levels[0] < 9)
            {
                Hierarchy._levelUpButton.interactable = false;
                Hierarchy._levelUpText.text +=
                    "<sprite name=\"Gold\"><color=#EB4154> x " + data.GoldCost[0].Value.ToString() + "</color>";
            }
            else
            {
                Hierarchy._levelUpButton.interactable = false;
                Hierarchy._levelUpText.text =
                    "Max Lvl";
            }
        }
        else
        {
            int classNumber = (int)data._class;

            if (data.GoldCost[classNumber].IsBay)
            {
                Hierarchy._levelUpButton.interactable = true;
                Hierarchy._levelUpText.text +=
                    "<sprite name=\"Gold\"> x " + data.GoldCost[classNumber].Value.ToString();
            }
            else
            {
                Hierarchy._levelUpButton.interactable = false;
                Hierarchy._levelUpText.text +=
                    "<sprite name=\"Gold\"><color=#EB4154> x " + data.GoldCost[classNumber].Value.ToString() + " </color>";
            }

            if(data.TokkenCost[classNumber].IsBay)
            {
                Hierarchy._levelUpText.text += " " + "<sprite name=\"Galeon\"> x " + data.TokkenCost[classNumber].Value.ToString();
            }
            else
            {
                Hierarchy._levelUpButton.interactable = false;
                Hierarchy._levelUpText.text +=
                    "<sprite name=\"Galeon\"><color=#EB4154> x " + data.TokkenCost[classNumber].Value.ToString() + "</color>";
            }

            if (data.levels[classNumber] >= 9)
            {
                Hierarchy._levelUpButton.interactable = false;
                Hierarchy._levelUpText.text =
                    "Max Lvl";
            }
        }
    }

    private void UpdateLevelNumber(MetaLevelingInfo data)
    {
        for(int i = 0; i < Hierarchy._classButtonsText.Length; i++)
        {
            Hierarchy._classButtonsText[i].text = "Lvl " + data.levels[i].ToString();
        }
    }

    Sequence sequenceBack = DOTween.Sequence();
    Sequence sequenceText = DOTween.Sequence();

    public void CannotBeReset()
    {
        sequenceBack = DOTween.Sequence();
        sequenceText = DOTween.Sequence();

        Color black = new Color(0, 0, 0, 0.8f);

        Hierarchy._cannotBeResetObjects.GetComponent<Image>().color = Color.clear;

        sequenceBack.AppendCallback(() => Hierarchy._closeCannotBeReset.gameObject.SetActive(true));
        sequenceBack.AppendCallback(() => Hierarchy._cannotBeResetObjects.SetActive(true));
        sequenceBack.Append(Hierarchy._cannotBeResetObjects.GetComponent<Image>().DOColor(black, 0.5f));
        sequenceBack.Join(Hierarchy._cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.white, 0.3f));
        sequenceBack.AppendInterval(1f);
        sequenceBack.Append(Hierarchy._cannotBeResetObjects.GetComponent<Image>().DOColor(Color.clear, 0.5f));
        sequenceBack.Join(Hierarchy._cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.clear, 0.5f));
        sequenceBack.AppendCallback(() => Hierarchy._cannotBeResetObjects.SetActive(false));
        sequenceBack.AppendCallback(() => Hierarchy._closeCannotBeReset.gameObject.SetActive(false));


        Hierarchy._cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.clear;

        //sequenceText.Append(Hierarchy._cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.white, 0.3f));
       // sequenceText.AppendInterval(1f);
       // sequenceText.Append(Hierarchy._cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.clear, 0.6f));

    }

    public void CannotBeResetFast()
    {
        sequenceBack.Kill();
        sequenceText.Kill();

        Hierarchy._closeCannotBeReset.gameObject.SetActive(false);

        sequenceBack.Append(Hierarchy._cannotBeResetObjects.GetComponent<Image>().DOColor(Color.clear, 0.2f));
        sequenceBack.AppendCallback(() => Hierarchy._cannotBeResetObjects.SetActive(false));

        sequenceText.Append(Hierarchy._cannotBeResetObjects.transform.GetChild(0).GetComponent<TMP_Text>().DOColor(Color.clear, 0.2f));
    }
}
