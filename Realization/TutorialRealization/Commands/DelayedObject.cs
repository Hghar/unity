using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extensions;
using Infrastructure.Helpers;
using UnityEngine;

namespace Realization.TutorialRealization.Commands
{
    public class DelayedObject : IObjectProvider<GameObject>, IDisposable
    {
        private string _name;
        private GameObject _object;
        private CancellationTokenSource _source = new();
        public string Name => _name;

        public DelayedObject(string name)
        {
            _name = name;
        }

        public GameObject Get()
        {
            if (_object != null) return _object;


            List<GameObject> objs = SceneObjectPool.Instance.Objects;//GameObject.FindObjectsOfType<GameObject>(true);

            foreach (GameObject gameObject in objs)
            {
                if (gameObject == null)
                    continue;
                
                Path path = gameObject.GetComponent<Path>();
                if (path != null)
                {
                    if (path.Full.EndsWith(_name))
                    {
                        _object = gameObject;
                        break;
                    }
                }
                else if (gameObject.gameObject.Path().EndsWith(_name))
                {
                    path = gameObject.AddComponent<Path>();
                    path.Init();
                    _object = gameObject;
                    break;
                }
            }

            return _object;
        }

        public async UniTask<GameObject> GetAsync()
        {
            while (_object == null)
            {
                if (_source.IsCancellationRequested)
                    return null;

                string[] parts = _name.Split('/');
                string fileName = parts[^1];
                List<GameObject> objects = SceneObjectPool.Instance.Objects;
                foreach (GameObject gameObject in objects)
                {
                    if (gameObject == null)
                        continue;
                    
                    Path path = gameObject.GetComponent<Path>();
                    if (path != null)
                    {
                        if (path.Full.EndsWith(_name))
                        {
                            _object = gameObject;
                            break;
                        }
                    }
                    else if (gameObject.gameObject.Path().EndsWith(_name))
                    {
                        path = gameObject.AddComponent<Path>();
                        path.Init();
                        _object = gameObject;
                        break;
                    }
                }
                await UniTask.WaitForEndOfFrame();
            }
            return _object;
        }

        public string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            if (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }

            return path;
        }

        public void Dispose()
        {
            _source.Cancel();
            _source?.Dispose();
        }
    }

    public class DelayedObject<T> : IObjectProvider<T>, IDisposable where T : Component
    {
        private string _name;
        private T _obj;
        private CancellationTokenSource _source = new();
        public string Name => _name;

        public DelayedObject(string name)
        {
            _name = name;
        }

        public T Get()
        {
            if (_obj == null)
            {
                string[] parts = _name.Split('/');
                string fileName = parts[^1];
                
                List<T> objs = SceneObjectPool.Instance.Objects
                    .Where((o => o != null && o.GetComponent<T>() != null))
                    .Select((o => o.GetComponent<T>()))
                    .ToList();
                // T[] objs = GameObject.FindObjectsOfType<T>(true);
                foreach (T component in objs)
                {
                    Path path = component.GetComponent<Path>();
                    if (path != null)
                    {
                        if (path.Full.EndsWith(_name))
                        {
                            _obj = component;
                            break;
                        }
                    }
                    else if (component.gameObject.Path().EndsWith(_name))
                    {
                        path = component.gameObject.AddComponent<Path>();
                        path.Init();
                        _obj = component;
                        break;
                    }
                }
            }

            return _obj;
        }

        public async UniTask<T> GetAsync()
        {
            if (_obj == null)
            {
                if (_source.IsCancellationRequested)
                    return null;

                string[] parts = _name.Split('/');
                string fileName = parts[^1];
                
                List<T> objs = SceneObjectPool.Instance.Objects
                    .Where((o => o != null && o.GetComponent<T>() != null))
                    .Select((o => o.GetComponent<T>()))
                    .ToList();
                // T[] objs = GameObject.FindObjectsOfType<T>(true);
                foreach (T component in objs)
                {
                    Path path = component.GetComponent<Path>();
                    if (path != null)
                    {
                        if (path.Full.EndsWith(_name))
                        {
                            _obj = component;
                            break;
                        }
                    }
                    else if (component.gameObject.Path().EndsWith(_name))
                    {
                        path = component.gameObject.AddComponent<Path>();
                        path.Init();
                        _obj = component;
                        break;
                    }
                }

                await UniTask.WaitForEndOfFrame();
            }

            return _obj;
        }

        public void Dispose()
        {
            _source.Cancel();
            _source?.Dispose();
        }
    }

    public class Path : MonoBehaviour
    {
        public string Full { get; private set; } = string.Empty;

        public void Init()
        {
            Full = gameObject.Path();
        }
    }
}