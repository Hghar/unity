using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Entities.ShopItems
{
    public class IconsProvider
    {
        private Dictionary<MinionClass, Sprite> _classIcons;
        private Dictionary<KeyValuePair<MinionClass, int>, Sprite> _mainIcons;
        private List<Sprite> _frames;

        public IconsProvider(
            Dictionary<MinionClass, Sprite> classIcons,
            Dictionary<KeyValuePair<MinionClass, int>, Sprite> mainIcons,
            List<Sprite> frames)
        {
            _mainIcons = mainIcons;
            _classIcons = classIcons;
            _frames = frames;
        }
        
        public Sprite FindIcon(MinionClass minionClass, int grade)
        {
            return _mainIcons[new KeyValuePair<MinionClass, int>(minionClass,grade)];
        }

        public Sprite FindClassIcon(MinionClass minionClass)
        {
            return _classIcons[minionClass];
        }

        public Sprite FindFrame(int characterGrade)
        {
            return _frames[characterGrade - 1];
        }
    }
}