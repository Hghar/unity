using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "New Minions Class Icons Library", menuName = "Icons/Class", order = 0)]
    public class MinionsClassIconsLibrary : ScriptableObject
    {
        [SerializeField] private List<ClassIconPair> _classIconPairs;

        private void OnValidate()
        {
            // TODO: remove duplicates;
        }

        public bool TryFindIcon(MinionClass minionClass, out Sprite iconSprite)
        {
            iconSprite = null;
            ClassIconPair classIconPair = _classIconPairs.Find(pair => pair.MinionClass == minionClass);
            if (classIconPair != null)
            {
                iconSprite = classIconPair.Sprite;
                return true;
            }

            return false;
        }

        [Serializable]
        private class ClassIconPair
        {
            [SerializeField] private Sprite _sprite;
            [SerializeField] private MinionClass _minionClass;

            public Sprite Sprite => _sprite;
            public MinionClass MinionClass => _minionClass;
        }
    }
}