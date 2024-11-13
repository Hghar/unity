using UnityEngine;

namespace Helpers
{
    public class BehaviourSwitcher : MonoBehaviour
    {
        [SerializeField] private Behaviour _behaviour;

        public void TestConstruct(Behaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void SwitchOn()
        {
            if(_behaviour == null)
                return;
            
            _behaviour.enabled = true;
        }

        public void SwitchOff()
        {
            if(_behaviour == null)
                return;
            
            _behaviour.enabled = false;
        }
    }
}