using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelEffects : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private SpriteRenderer _arrow;
    [SerializeField] private SpriteRenderer _light;

    private GameObject _parentTextObject;
    private GameObject _parentArrowObject;
    private GameObject _parentLightObject;

    private void OnEnable()
    {
        _parentTextObject = _text.gameObject;
        _parentArrowObject = _arrow.gameObject;
        _parentLightObject = _light.gameObject;

        _parentTextObject.SetActive(false);
        _parentArrowObject.SetActive(false);
        _parentLightObject.SetActive(false);
    }
    public void AddExpEffect(int exp)
    {
        GameObject addExpObject = GameObject.Instantiate(_parentTextObject, this.transform.parent);

        addExpObject.transform.localPosition = new Vector2(0, 2f);

        var sequance = DOTween.Sequence();

        float time = 1f;

        Color32 startColor = new Color32(250, 191, 63, 0);
        Color32 endColor = new Color32(250, 191, 63, 255);

        _text = addExpObject.GetComponent<TMP_Text>();

        _text.text = $"+ {exp} Exp";

        _text.color = startColor;

        addExpObject.SetActive(true);

        sequance.Append(_text.gameObject.transform.DOLocalMoveY(2.2f, time));
        sequance.Join(_text.DOColor(endColor, time / 3));
        sequance.Join(_text.DOColor(startColor, time / 2).SetDelay(time/3));
        sequance.OnComplete(() => Destroy(addExpObject));

        sequance.Play();
    }

    public void LevelUpEffect()
    {
        GameObject textObject = GameObject.Instantiate(_parentTextObject, this.transform.parent);
        GameObject arrowObject = GameObject.Instantiate(_parentArrowObject, this.transform.parent);
        GameObject lightObject = GameObject.Instantiate(_parentLightObject, this.transform.parent);

        TMP_Text text = textObject.GetComponent<TMP_Text>();
        SpriteRenderer arrow = arrowObject.GetComponent<SpriteRenderer>();
        SpriteRenderer light = lightObject.GetComponent<SpriteRenderer>();

        textObject.transform.localPosition = new Vector2(0, 0.7f);
        arrowObject.transform.localPosition = new Vector2(0, 0.9f);
        lightObject.transform.localPosition = new Vector2(0, 0.6f);

        var sequance = DOTween.Sequence();

        float time = 0.5f;

        Color32 startColor = new Color32(250, 191, 63, 0);
        Color32 endColor = new Color32(250, 191, 63, 150);

        Vector2 startArrowScale = new Vector2(_parentArrowObject.transform.localScale.x * (0.1f), _parentArrowObject.transform.localScale.y * (0.1f));
        Vector2 endArrowScale = new Vector2(_parentArrowObject.transform.localScale.x, _parentArrowObject.transform.localScale.y);

        Vector2 startLightScale = new Vector2(_parentLightObject.transform.localScale.x * (0.1f), _parentLightObject.transform.localScale.y * (0.1f));
        Vector2 endLightScale = new Vector2(_parentLightObject.transform.localScale.x, _parentLightObject.transform.localScale.y);

        Vector2 startArrowPosition = arrowObject.transform.localPosition - (Vector3.up * 0.500f);
        Vector2 endArrowPosition = arrowObject.transform.localPosition;

        text = textObject.GetComponent<TMP_Text>();
        arrow = arrowObject.GetComponent<SpriteRenderer>();
        light = lightObject.GetComponent<SpriteRenderer>();

        text.text = "Lvl UP";

        light.color = startColor;
        text.color = startColor;

        arrow.transform.localScale = startArrowScale;
        arrow.transform.localPosition = startArrowPosition;

        lightObject.transform.localScale = startLightScale;

        textObject.SetActive(true);
        arrowObject.SetActive(true);
        lightObject.SetActive(true);

        sequance.Append(light.DOColor(endColor, 0.5f));
        sequance.Join(text.DOColor(endColor, 0.5f));
        sequance.Join(arrowObject.transform.DOScale(endArrowScale, time));
        sequance.Join(arrowObject.transform.DOLocalMoveY(endArrowPosition.y, time));
        sequance.Join(lightObject.transform.DOScale(endLightScale, time));
        sequance.Append(arrow.DOColor(startColor, 0.5f));
        sequance.Join(text.DOColor(startColor, 0.5f));
        sequance.Join(light.DOColor(startColor, 0.5f));
        sequance.OnComplete(() => { Destroy(arrowObject); Destroy(lightObject); Destroy(textObject); });

        sequance.Play();
    }
}
