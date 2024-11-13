using System;
using UnityEngine;

namespace Realization.DevTools
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            Debug.LogError(gameObject.name);
            DontDestroyOnLoad(gameObject);
        }
    }
}