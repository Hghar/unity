using Infrastructure.Services.WindowService.MVVM;
using UnityEngine;

namespace Infrastructure.Services.WindowService.ViewFactory
{
    public interface IViewFactory
    {
        TView CreateView<TView, THierarchy>(THierarchy hierarchy)
                where TView : View<THierarchy>
                where THierarchy : MonoBehaviour;

        TView CreateView<TView, THierarchy>(string prefabName, Transform parent)
                where TView : View<THierarchy>
                where THierarchy : MonoBehaviour;
        TView CreateView<TView, THierarchy>(GameObject prefab, Transform parent)
                where TView : View<THierarchy>
                where THierarchy : MonoBehaviour;
    }
}