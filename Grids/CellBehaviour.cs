using System;
using UnityEngine;

namespace Grids
{
    public class CellBehaviour : MonoBehaviour
    {
        public float Size;
        public Collider2D Collider2D;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position,  Vector3.one*Size);
        }
    }
}