using Cysharp.Threading.Tasks;
using Plugins.Ship.Sheets;
using UnityEngine;

namespace Realization.States.EmptySheetHelper
{
    public class EmptySheet : ISheet
    {
        public bool Working => false;
        public string Name => "Empty sheet";

        public void Start()
        {
        }

        public UniTask Update(MonoBehaviour coroutineRunner)
        {
            return UniTask.CompletedTask;
        }
    }
}