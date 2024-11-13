using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Services.WindowService.MVVM
{
    public class AlligmentHelper: MonoBehaviour
    {
        [SerializeField] private List<Point> _points = new List<Point>();
        public Vector2 GetPosition(Alignment alignment) => 
                _points.First(x => x.Alignment == alignment).Transform.position;

        public List<Point> GetPoints() => _points;
    }
}