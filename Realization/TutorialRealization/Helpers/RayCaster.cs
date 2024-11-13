using Infrastructure.RayCastingEssence;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Realization.TutorialRealization.Helpers
{
    public class RayCaster : MonoBehaviour
    {
        private const string Layer = "Clickable";

        private RayCasting _rayCasting;

        [Inject]
        private void Construct(RayCasting rayCasting)
        {
            _rayCasting = rayCasting;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Transform[] targets = _rayCasting.CastAll<Transform>();
                foreach (Transform handler in targets)
                {
                    if ((handler).gameObject.layer == LayerMask.NameToLayer(Layer))
                    {
                        handler.GetComponent<IPointerClickHandler>()
                            ?.OnPointerClick(new PointerEventData(EventSystem.current));
                    }
                }
            }
        }
    }
}