using UnityEngine;

namespace UI.Map
{
    public interface IMapTileViewFactory
    {
        public void Create(Transform parent, IMapTileViewInfo info);
    }
}