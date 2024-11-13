using System;
using Model.Economy;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Realization.States
{
    public class StateWorker : MonoBehaviour,ILateTickable
    {
        [SerializeField] private StateInitializer _state;
        
        private IStorage _storage;
        private bool _started;
        private bool inited;

        [Inject]
        private void Construct(IStorage storage)
        {
            _storage = storage;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Init()
        {
            inited = true;
            if(_storage == null)
                return;
            
            _started = _storage.PlayerProgress.TutorialData.Started;
            
            // if (SceneManager.GetActiveScene().name == "Main")
            // {
            //     _storage.PlayerProgress.TutorialData.Started = true;
            // }
            // DontDestroyOnLoad(gameObject);
        }

       

        public void LateTick()
        {
            // if (!inited) return;
            // if(_storage == null)
            //     return;
            //
            // if(_started == false)
            //     _state.UpdateState();
        }
    }
}