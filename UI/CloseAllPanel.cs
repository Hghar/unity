using System.Collections.Generic;
using Units;
using Units.Picking;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseAllPanel : MonoBehaviour
{
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] private InfoHint _infoHint;
    private IPickableUnit _unpickUnit;

    public void Inicialize(IMinion minion)
    {
        _unpickUnit = minion.GameObject.transform.Find("Pickable").GetComponent<PickableUnit>();
    }
    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] RectTransform canvasRect;

    void Update()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the game object
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (Input.GetMouseButtonDown(0))
        {
            if (results.Count > 0) return;
            if (_unpickUnit != null && (canvasRect.gameObject.transform.GetChild(1).gameObject.activeSelf ||
                                        canvasRect.gameObject.transform.GetChild(0).GetChild(1).gameObject
                                                .activeSelf))
            {
                _infoHint.CloseOnButton();
                _unpickUnit.Unpick();
            }
            else if (canvasRect.gameObject.transform.GetChild(1).gameObject.activeSelf ||
                     canvasRect.gameObject.transform.GetChild(0).GetChild(1).gameObject.activeSelf)
            {
                _infoHint.CloseOnButton();
            }
        }
    }
    public void Close()
    {
        if (_unpickUnit != null)
        {
            _unpickUnit.Unpick();
        }
    }
}
