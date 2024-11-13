using UnityEngine;
using Zenject;

namespace Grids
{
    public class TestGridObjectBehaviour : MonoBehaviour
    {
        [SerializeField] private GridBehaviour _behaviour;

        public void SetOnPosition(Vector2Int position)
        {
            transform.position = _behaviour.ToWorld(position);
        }
    }
}