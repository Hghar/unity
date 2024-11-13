using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Units;

public class ControlBars : MonoBehaviour
{
    [SerializeField] private Slider _HPBar;
    [SerializeField] private Slider _shieldBar;
    [SerializeField] private Slider _energyBar;
    [SerializeField] private GameObject _levelIcon;

    [SerializeField] private Image _colorImage;
    [SerializeField] private Color32 _minionHPBarColor;

    [SerializeField] private Sprite[] _classIcons;

    private void OnEnable()
    {
        _HPBar.transform.parent.parent.GetComponent<Canvas>().worldCamera = Camera.main;

        StartCoroutine(UpdateBar());
    }
    private IEnumerator UpdateBar()
    {
        yield return new WaitForEndOfFrame();

        IMinion minion = this.transform.parent.GetComponent<IMinion>();

        if (minion.Fraction == Fight.Fractions.Fraction.Minions)
        {
            var colors = _HPBar.colors;
            colors.normalColor = _minionHPBarColor;

            _colorImage.color = _minionHPBarColor;
        }

        UpdateIconLevel(_classIcons[(int)minion.Class]);
    }

    public void EnableEnergyBar()
    {
        _energyBar.gameObject.SetActive(true);
    }
    private void Update()
    {
        Vector2 newPos = new Vector3(
            transform.parent.transform.position.x, 
            transform.parent.transform.position.y + 1.7f);
        UpdatePosition(newPos);
    }
    private void UpdatePosition(Vector2 newPosition)
    {
        _HPBar.gameObject.transform.parent.transform.position = newPosition;
    }
    public void UpdateHPBar(float newHP, float maxHP)
    {
        _HPBar.value = (1 / maxHP * newHP);
    }
    
    public void UpdateShieldBar(float newShield, float maxShield)
    {
        _shieldBar.value = (1 / maxShield * newShield);
    }

    public void BarsDisabled()
    {
        _HPBar.transform.parent.parent.gameObject.SetActive(false);
    }

    public void UpdateEnergyBar(float newEnergy, float maxEnergy)
    {
        _energyBar.value = (1 / maxEnergy * newEnergy);
    }

    public void SetEnergyBarActive(bool active)
    {
        _energyBar.gameObject.SetActive(active);
    }

    public void UpdateIconLevel(Sprite sprite)
    {
        _levelIcon.GetComponent<Image>().overrideSprite = sprite;
        _levelIcon.GetComponent<Image>().sprite = sprite;
    }
    public void UpdateLevelNumber(int level)
    {
        _levelIcon.transform.GetChild(1).GetComponent<TMP_Text>().text = level.ToString();
    }

    public void SetLevelActive(bool workingFeature)
    {
        _levelIcon.gameObject.SetActive(workingFeature);
    }
}
