using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopParticlContainer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _gradientPaticl;
    [SerializeField] private ParticleSystem _Particl;

    public void UpdateColor(Color32 color)
    {
        //_gradientPaticl.gameObject.SetActive(false);
        var main = _gradientPaticl.main;

        Color32 backcolor = new Color32(color.r, color.g, color.b, (byte)(main.startColor.color.a * 255));

        main.startColor = new ParticleSystem.MinMaxGradient(backcolor);
        //_gradientPaticl.gameObject.SetActive(true);

        //_Particl.gameObject.SetActive(false);
        main = _Particl.main;
        main.startColor = new ParticleSystem.MinMaxGradient(color);
        //_Particl.gameObject.SetActive(true);

    }
}
