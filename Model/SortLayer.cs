using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SortLayer : MonoBehaviour
{
    [SerializeField] private Transform _characterParent;
    [SerializeField] private Transform _parentTransform;
    // Update is called once per frame
    void FixedUpdate()
    {
        float sortingParam = -_parentTransform.position.y + 100;
        try
        {
            _characterParent.GetChild(0).GetChild(0).GetChild(0).GetComponent<SortingGroup>().sortingOrder = (int)sortingParam;
        }
        catch
        {
            Debug.Log("Child Null");
        }
    }
}
