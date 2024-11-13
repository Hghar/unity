using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCharacterParticl : MonoBehaviour
{
    [SerializeField] private GameObject[] _particlObjects;
    [SerializeField] private Color32[] _colors;

    [SerializeField] private GameObject _particlClass;
    [SerializeField] private Color32[] _classColors;
    public void SetActiveHintEffect(bool isActive, int grade = 0)
    {
        for(int i = 0; i < _particlObjects.Length; i++)
        {
            if(_particlObjects[i] != null)
                _particlObjects[i].SetActive(isActive);

            if (grade > 0 && isActive)
            {
                _particlObjects[i].GetComponent<ShopParticlContainer>().UpdateColor(_colors[grade - 1]);
            }
        }
    }

    public void SetActiveClassEffect(bool isActive, int Class = -1)
    {
        if(_particlClass != null)
            _particlClass.SetActive(isActive);

        if(Class >= 0)
            _particlClass.GetComponent<ShopParticlContainer>().UpdateColor(_classColors[Class]);
    }
}
