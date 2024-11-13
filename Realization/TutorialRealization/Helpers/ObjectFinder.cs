using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Realization.TutorialRealization.Helpers
{
    public class ObjectFinder : MonoBehaviour
    {
        [SerializeField] private GameObject _fade;
        [SerializeField] private GameObject _hardTutorial;
        [SerializeField] private Transform _handWorld;
        [SerializeField] private Transform _handCamera;
        [SerializeField] private Transform _handOverlay;
        [SerializeField] private RawImage _overlayScreen;

        private RenderTexture _texture;
        private TutorialCameraService _tutorialCameraService;
        public RenderTexture Texture => _texture;


        // [Inject]

        // private void Construct(TutorialCameraService tutorialCameraService)

        // {

        //     _tutorialCameraService = tutorialCameraService;

        // }


        private void Awake()
        {
            _texture = new ( Screen.width, Screen.height, 24);
            _overlayScreen.texture = _texture;
            Find<Camera>("TutorialCamera").targetTexture = _texture;
            // _tutorialCameraService.ChangeTargetTexture(_texture);
        }

        public T Find<T>(string name) where T : Component
        {
            T found = GameObject.Find(name)?.GetComponent<T>();
            if (found != null)
                return found;

            throw new Exception($"Object with name {name} not found!");
        }

        public GameObject Fade => _fade;
        public GameObject HardTutorial => _hardTutorial;
        public TutorialHand Hand => new TutorialHand(_handWorld, _handCamera, _handOverlay);
    }
}