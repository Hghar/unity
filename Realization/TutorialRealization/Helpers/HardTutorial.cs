using System;
using System.Collections.Generic;
using System.Linq;
using CustomInput;
using CustomInput.Picking;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Realization.TutorialRealization.Helpers
{
    public class HardTutorial : MonoBehaviour
    {
        [SerializeField] private Transform _mask;
        [SerializeField] private Transform _excludesParent;
        [SerializeField] private RectTransform _worldCanvas;
        [SerializeField] private Transform _excludesParentAnother;

        private readonly List<HardTutorialObject> _excluded = new();
        private readonly Dictionary<GameObject, GameObject> _fadeExcluded = new();
        private readonly List<GameObject> _deactivatedUnits = new();

        public static bool Activated { get; private set; }
        public static HardTutorial Instance { get; private set; }

        private void Awake()
        {
            if(Instance == null)
                Instance = this;
        }

        public void ExcludeFromFade(GameObject gameObject)
        {
            GameObject copy = null;

            if (gameObject.transform is RectTransform)
            {
                copy = CreateCopy(gameObject);
                _fadeExcluded.Add(gameObject, copy);
            }
        }
        
        public void IncludeInFade(GameObject gameObject)
        {
            if(_fadeExcluded.ContainsKey(gameObject) == false)
                return;
            
            var copy = _fadeExcluded[gameObject];
            if (copy == null)
                return;
            
            Destroy(copy);
            _fadeExcluded.Remove(gameObject);
        }

        public void Exclude(GameObject gameObject)
        {
            GameObject copy = null;
            
            if (gameObject.transform is RectTransform)
            {
                copy = CreateCopy(gameObject);
            }

            TryEnableHoldAndTap(gameObject);

            HardTutorialObject excluded = new HardTutorialObject()
            {
                Parent = gameObject.transform.parent,
                GameObject = gameObject,
                Layer = gameObject.layer,
                Copy = copy,
            };

            gameObject.layer = LayerMask.NameToLayer("Clickable");
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if(child.gameObject.layer != LayerMask.NameToLayer("Pickable"))
                    child.gameObject.layer = LayerMask.NameToLayer("Clickable");
            }

            List<IPointerClickHandler> buttons = gameObject.GetComponentsInChildren<IPointerClickHandler>().ToList();
            buttons.Add(gameObject.GetComponent<IPointerClickHandler>());
            foreach (IPointerClickHandler button in buttons)
            {
                MonoBehaviour mono = (button as MonoBehaviour);
                if (mono != null)
                    mono.enabled = true;
            }

            _excluded.Add(excluded);
        }

        private void TryEnableHoldAndTap(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Draggable>(out var draggable))
            {
                draggable.Working = true;
                _deactivatedUnits.Add(gameObject);
            }

            var pickable = gameObject.GetComponentInChildren<Pickable>();
            if (pickable != null)
            {
                pickable.Working = true;
                _deactivatedUnits.Add(gameObject);
            }
        }

        private GameObject CreateCopy(GameObject main)
        {
            GameObject copy;
            if (main.GetComponentInParent<Canvas>() != null &&
                main.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceOverlay)
            {
                copy = Instantiate(main, main.transform.parent);
                copy.transform.SetParent(_excludesParent);
            }
            else
            {
                copy = Instantiate(main,  main.transform.parent);
                copy.transform.SetParent(_excludesParentAnother);
            }
            
            copy.name = $"Copy_{copy.name}_Copy";
            copy.SetActive(true);
            copy.AddComponent<Clone>().Init(main);

            List<Component> components = copy.GetComponents<Component>().ToList();
            components.AddRange(copy.GetComponentsInChildren<Component>());

            foreach (Component component in components)
            {
                if (component is not CanvasRenderer &&
                    component is not Image &&
                    component is not RectTransform &&
                    component is not Transform &&
                    component is not SpriteRenderer &&
                    component is not TMP_Text &&
                    component is not Button &&
                    component is not Clone)
                {
                    Destroy(component);
                }

                if (component is Button)
                {
                    // component.GetComponent<Button>().enabled = true;
                }
            }

            return copy;
        }

        public void Include(GameObject gameObject)
        {
            List<IPointerClickHandler> buttons;
            foreach (HardTutorialObject tutorialObject in _excluded)
            {
                if (gameObject != null && gameObject == tutorialObject.GameObject)
                {
                    _excluded.Remove(tutorialObject);
                    gameObject.transform.SetParent(tutorialObject.Parent);
                    gameObject.layer = tutorialObject.Layer;
                    if(tutorialObject.Copy != null)
                        Destroy(tutorialObject.Copy);
                    Transform[] children = gameObject.GetComponentsInChildren<Transform>();
                    foreach (Transform child in children)
                    {
                        child.gameObject.layer = tutorialObject.Layer;
                    }

                    buttons = gameObject.GetComponentsInChildren<IPointerClickHandler>().ToList();
                    buttons.Add(gameObject.GetComponent<IPointerClickHandler>());
                    foreach (IPointerClickHandler button in buttons)
                    {
                        if (button == null)
                            continue;
                        (button as MonoBehaviour).enabled = false;
                    }

                    return;
                }
            }
            
            buttons = gameObject.GetComponentsInChildren<IPointerClickHandler>().ToList();
            buttons.Add(gameObject.GetComponent<IPointerClickHandler>());
            foreach (IPointerClickHandler button in buttons)
            {
                if (button == null)
                    continue;
                (button as MonoBehaviour).enabled = false;
            }

            if (gameObject.TryGetComponent<Draggable>(out var draggable))
            {
                gameObject.GetComponentInChildren<Pickable>().Working = false;
                draggable.Working = false;
            }

            Debug.LogError(
                $"GameObject {gameObject.name} not found in excluded. " +
                $"If you know you haven't added it, then ignore");
        }

        public void Clear()
        {
            // foreach (var unit in _deactivatedUnits)
            // {
            //     if (unit != null)
            //     {
            //         TryEnableHoldAndTap(unit);   
            //     }
            // }
            // _deactivatedUnits.Clear();
            // Deactivate();
            var draggales = FindObjectsOfType<Draggable>();
            foreach (var draggale in draggales)
            {
                draggale.GetComponentInChildren<Pickable>().Working = false;
                draggale.Working = false;
            }
            
            _excluded.RemoveAll((o => o.GameObject == null));
            foreach (var tutorialObject in _excluded.ToArray())
            {
                Include(tutorialObject.GameObject);
            }
            _excluded.Clear();
        }

        public void Activate()
        {
            _mask.gameObject.SetActive(true);
            Transform[] objs = FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objs)
            {
                List<Button> buttons = obj.GetComponentsInChildren<Button>(true).ToList();
                buttons.Add(obj.GetComponent<Button>());
                foreach (Button button in buttons)
                {
                    if (button == null)
                        continue;

                    button.enabled = false;
                }
            }
            
            List<IPointerClickHandler> buttons1;
            buttons1 = gameObject.GetComponentsInChildren<IPointerClickHandler>().ToList();
            buttons1.Add(gameObject.GetComponent<IPointerClickHandler>());
            foreach (IPointerClickHandler button in buttons1)
            {
                if (button == null)
                    continue;
                (button as MonoBehaviour).enabled = true;
            }

            Activated = true;
        }

        public void Deactivate()
        {
            _mask.gameObject.SetActive(true);
            Transform[] objs = FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objs)
            {
                List<Button> buttons = obj.GetComponentsInChildren<Button>(true).ToList();
                buttons.Add(obj.GetComponent<Button>());
                foreach (Button button in buttons)
                {
                    if (button == null)
                        continue;

                    button.enabled = true;
                }
            }
            Activated = false;
        }

        public void ClearFade()
        {
            foreach (var copy in _fadeExcluded)
            {
                Destroy(copy.Value);
            }
            _fadeExcluded.Clear();

            foreach (var hardExcluded in _excluded)
            {
                Destroy(hardExcluded.Copy);
            }
        }

        public void DisableMinion(GameObject minionObject)
        {
            var draggale = minionObject.GetComponent<Draggable>();
            draggale.GetComponentInChildren<Pickable>().Working = false;
            draggale.Working = false;
        }
    }

    public struct HardTutorialObject
    {
        public Transform Parent;
        public GameObject GameObject;
        public int Layer;
        public GameObject Copy;
    }
}