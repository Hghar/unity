using System;
using UnityEngine;

namespace Realization.TutorialRealization.Helpers
{
    public class TutorialHelpers : MonoBehaviour
    {
        [SerializeField] private HardTutorial _hardTutorial;
        [SerializeField] private ObjectFinder _objectFinder;

        public HardTutorial Tutorial => _hardTutorial;

        public ObjectFinder Finder => _objectFinder;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}