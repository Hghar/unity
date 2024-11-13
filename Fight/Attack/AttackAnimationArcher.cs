using System;
using System.Collections;
using AssetStore.HeroEditor.Common.CharacterScripts;
using Events;
using UnityEngine;


namespace Fight.Attack
{
    public class AttackAnimationArcher : MonoBehaviour
    {
        [SerializeField] private Transform _startPosBullet;
        [SerializeField] private GameObject _characterParent;
    
        [Header("Check to disable arm auto rotation.")]
        public bool FixedArm;
    
        private float _delayAnimation = 0.4f;
        private float _chargeTime;
        private Transform arm;
        private GameObject _addScriptOnAnimationObject;

        public Vector3 StartPosBullet 
        {
            get
            {
                Vector3 retPosition = _characterParent.transform.position + Vector3.up;

                return retPosition;
            }
        }
    
        private void Start()
        {
            if (_startPosBullet == null)
            {
                _startPosBullet = _characterParent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform
                    .GetChild(0).transform.GetChild(2).transform.GetChild(2).transform.GetChild(1).GetComponent<Transform>();
            }

            _addScriptOnAnimationObject = _characterParent.transform.GetChild(0).transform.GetChild(0).gameObject;
            _addScriptOnAnimationObject.AddComponent<RangeEventHandler>();
        }
    
        public void ShootAnimation(Action callback)
        {
            _characterParent.transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>()
                .SetTrigger("SimpleBowShot");
            callback.Invoke();
        }
    }
}
