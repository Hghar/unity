using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Infrastructure.Helpers
{
    public class SceneObjectPool
    {
        private static SceneObjectPool _instance;

        public static SceneObjectPool Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new SceneObjectPool();
                    Init();
                }

                return _instance;
            }
        }

        public List<GameObject> Objects = new ();

        public static void Init()
        {
            var buttons = Object.FindObjectsOfType<GameObject>(true);
            Instance.Objects.AddRange(buttons);
        }

        public static void AddRange(GameObject[] objects)
        {
            Instance.Objects.AddRange(objects);
        }

        public void AddScene()
        {
            Init();
        }
    }
}