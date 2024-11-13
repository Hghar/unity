using Infrastructure.Helpers;
using Infrastructure.Services.WindowService.MVVM;
using Infrastructure.Services.WindowService.PrefabFactory;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.WindowService.ViewFactory
{
    internal sealed class ViewFactory : IViewFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IPrefabFactory _prefabFactory;

        public ViewFactory(IInstantiator instantiator, IPrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
            _instantiator = instantiator;
        }

        public TView CreateView<TView, THierarchy>(THierarchy hierarchy)
                where TView : View<THierarchy>
                where THierarchy : MonoBehaviour
        {
            var view = _instantiator.Instantiate<TView>(new object[]{hierarchy});
            return view;
        }

        public TView CreateView<TView, THierarchy>(string prefabName, Transform parent)
                where TView : View<THierarchy>
                where THierarchy : MonoBehaviour
        {
            var hierarchy = _prefabFactory.Instantiate<THierarchy>(prefabName, parent);

            return CreateView<TView, THierarchy>(hierarchy);
        }
        public TView CreateView<TView, THierarchy>(GameObject prefabName, Transform parent)
                where TView : View<THierarchy>
                where THierarchy : MonoBehaviour
        {
            var instantiate = Object.Instantiate(prefabName,parent);
            SceneObjectPool.Instance.Objects.Add(instantiate);
            foreach (Transform child in instantiate.transform.GetComponentsInChildren<Transform>()) 
            {
                SceneObjectPool.Instance.Objects.Add(child.gameObject);
            }
            var hierarchy = instantiate.GetComponent<THierarchy>();
            return CreateView<TView, THierarchy>(hierarchy);
        }
    }
}