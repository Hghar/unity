using System;
using UnityEngine;

namespace Helpers
{
    public class TutorialCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Update()
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
                return;
            }
            
            transform.position = _mainCamera.transform.position;
        }
    }
}