using System;
using System.Collections.Generic;
using Picking;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace CustomInput.Picking
{
    public class Pickable : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IPickingPublisher
    {
        [SerializeField] private PickMarker _marker;

        private bool _isPicked = false;
        private bool _isDragged;

        public bool IsMainPickable = false;

        public event Action Picked;
        public event Action Unpicked;

        private InfoHint _infoHint;

        private CharacterOnSceneInformation test;

        public bool Working { get; set; } = true;

        [Inject]
        private void Construct(InfoHint infoHint)
        {
            _infoHint = infoHint;

        }

        public void RegisterInfoHint(MinionClass minionClass, int minionGrade)
        {
            test = new CharacterOnSceneInformation(minionClass,minionGrade);
            _infoHint.clickButton += PickInfoHint;
            _infoHint.unclickButton += UnPickInfoHint;
            _infoHint.AddMininon(test);
        }

        private void OnDestroy()
        {
            _infoHint.clickButton -= PickInfoHint;
            _infoHint.unclickButton -= UnPickInfoHint;

            if (this.transform.parent.GetComponent<IMinion>().Fraction == Fight.Fractions.Fraction.Minions)
            {
                _infoHint.RemoveMinion(test);
            }
        }

        private void Awake()
        {
            _marker.SwitchOff();
        }

        public void Init(IEnumerable<Vector2Int> shape) // TODO: Separate marker initing to other class
        {
            _marker.Init(shape);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(Working == false)
                return;
            
            Picker();

            IsMainPickable = true;
        }

        private void Picker()
        {
            if (_isDragged == false)
            {
                //InfoHintMarker(_isPicked);

                if (_isPicked)
                {
                    Unpick();
                }
                else
                {
                    Pick(this.transform.parent.gameObject.GetComponent<IMinion>().Class);
                }
            }
        }

        private void InfoHintMarker(bool active)
        {
            GameObject parentObject = this.transform.parent.parent.gameObject;

            if (active)
            {
                for (int i = 0; i < parentObject.transform.childCount; i++)
                {
                    if (parentObject.transform.GetChild(i).gameObject == this.transform.parent.gameObject)
                    {
                        continue;
                    }

                    if (parentObject.transform.GetChild(i).GetComponent<IMinion>().Fraction == this.transform.parent.GetComponent<IMinion>().Fraction)
                    {
                        if (parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().IsMainPickable)
                        {
                            parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().Unpick();
                        }

                        if (parentObject.transform.GetChild(i).GetComponent<IMinion>().Class == this.transform.parent.GetComponent<IMinion>().Class)
                        {
                            parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().MarkerOff();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < parentObject.transform.childCount; i++)
                {
                    if (parentObject.transform.GetChild(i).gameObject == this.transform.parent.gameObject)
                    {
                        continue;
                    }

                    if (parentObject.transform.GetChild(i).gameObject.GetComponent<IMinion>().Fraction == this.transform.parent.gameObject.GetComponent<IMinion>().Fraction)
                    {
                        if (parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().IsMainPickable)
                        {
                            parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().Unpick();
                        }

                        if (parentObject.transform.GetChild(i).gameObject.GetComponent<IMinion>().Class == this.transform.parent.gameObject.GetComponent<IMinion>().Class)
                        {
                            parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().MarkerOn();
                        }
                        else
                        {
                            parentObject.transform.GetChild(i).GetChild(6).GetComponent<Pickable>().MarkerOff();
                        }
                    }
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragged = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragged = false;
        }

        public void Unpick()
        {
            if (_marker != null)
                MarkerOff();

            Unpicked?.Invoke();

            IsMainPickable = false;
        }

        public void MarkerOn()
        {
            if (_isPicked == false)
            {
                _isPicked = true;

                _marker.SwitchOn();
            }
        }

        public void MarkerOff()
        {
            if (_isPicked == true)
            {
                _isPicked = false;

                _marker.SwitchOff();
            }
        }

        private void PickInfoHint(MinionClass minionClass)
        {
            if (minionClass == this.transform.parent.gameObject.GetComponent<IMinion>().Class) 
            {
                _marker.SwitchOn();
            }
            else
            {

                _marker.SwitchOff();

            }
        }

        private void UnPickInfoHint()
        {
            _marker.SwitchOff();
        }

        private void Pick(MinionClass minionClass)
        {
            if (minionClass == this.transform.parent.gameObject.GetComponent<IMinion>().Class)
            {
                MarkerOn();
                Picked?.Invoke();
            }
        }
    }
}