using NaughtyAttributes;
using UnityEngine;

namespace Realization.Utils
{
    public class ByCameraModifier : MonoBehaviour
    {
        [SerializeField] private bool _scale;
        [EnableIf("_scale")] [SerializeField] private bool _yScale;
        [SerializeField] private bool _position;

        private bool _isScaleModified;
        private bool _isInited;
        private float _modificator;

        public Vector3 Scale => transform.localScale;

        private void Awake()
        {
            TryInit();

            if (_scale)
                TryModifyScale();

            if (_position)
            {
                Vector3 scale = transform.localPosition;
                scale.x *= _modificator;
                transform.localPosition = scale;
            }
        }

        public bool TryModifyScale()
        {
            if (_isInited == false)
                return false;

            if (_isScaleModified)
                return false;

            Vector3 scale = transform.localScale;
            scale.x *= _modificator;
            if (_yScale)
                scale.y *= _modificator;
            transform.localScale = scale;
            _isScaleModified = true;
            return true; 
        }

        public bool TryInit()
        {
            if (_isInited)
                return false;

            float aspect = ((float)Screen.width) / Screen.height;
            _modificator = aspect / ScreenUtils.DefaultAspect;
            _isInited = true;
            return true;
        }
    }
}