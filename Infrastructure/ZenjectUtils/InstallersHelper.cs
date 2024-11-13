using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

#pragma warning disable CS1030
#pragma warning disable CS0414

namespace Infrastructure.ZenjectUtils
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
    public class InstallersHelper : MonoBehaviour
    {
        [Required] [SerializeField] private SceneContext _sceneContext;

        [ShowIf(EConditionOperator.And, nameof(DontHaveDuplicates), nameof(DontHaveEmptyObjects))]
        [HorizontalLine(2f, EColor.Green)]
        [ReadOnly]
        [Label("STATUS:")]
        [SerializeField]
        private string _status = "OK";

        [HideIf(nameof(DontHaveDuplicates))]
        [BoxGroup("Errors")]
        [HorizontalLine(2f, EColor.Red)]
        [ResizableTextArea]
        [ReadOnly]
        [SerializeField]
        private string _duplicatesInfo = "";

        [HideIf(nameof(DontHaveEmptyObjects))]
        [BoxGroup("Warnings")]
        [HorizontalLine(2f, EColor.Yellow)]
        [ResizableTextArea]
        [ReadOnly]
        [SerializeField]
        private string _emptyObjectsInfo = "";

        private bool DontHaveDuplicates()
        {
            return _duplicatesInfo == "";
        }

        private bool DontHaveEmptyObjects()
        {
            return _emptyObjectsInfo == "";
        }

        private void Update()
        {
            _sceneContext ??= GetComponent<SceneContext>();

            //TODO: dont delete empty objects if they have children with installer
            //TODO: rework status
            //TODO: check if some installers not found but using
            //TODO: auto finding needed installers and button add if its needed
            //TODO: debug if something wrong
            //TODO: show all installers in the project
            //TODO: separate view and logic
            //TODO: add auto mode
            //TODO: add unit tests

            LogDuplicates();
            LogEmpty();
        }

        private void LogDuplicates()
        {
            _duplicatesInfo = "";
            foreach (MonoInstaller installer in FindDuplicates(transform))
            {
                _duplicatesInfo += $"Copy: {installer.GetType().Name}\n" +
                                   $"Path: {GetFullPath(installer.transform)}\n\n";
            }
        }

        private void LogEmpty()
        {
            IEnumerable<Transform> children = AllChildren(transform).Where(child => child != null);
            _emptyObjectsInfo = "";
            foreach (Transform child in children)
            {
                if (child.TryGetComponent(out MonoInstaller installer) == false)
                    _emptyObjectsInfo += $"Empty: {child.name}\n" +
                                         $"Path: {GetFullPath(child)}\n\n";
            }
        }

        [Button]
        private void DeleteDuplicates() // TODO: button sets installers into context, but name doesn't represent this
        {
            List<MonoInstaller> installers = new List<MonoInstaller>();

            DeleteDuplicates(transform, installers);

            _sceneContext.Installers = installers;
        }

        [Button]
        private void DeleteObjectsWithoutInstaller()
        {
            DeleteObjectsWithoutInstaller(transform);
        }

        private void DeleteDuplicates(Transform parent, List<MonoInstaller> installers)
        {
            IEnumerable<MonoInstaller> children = AllChildren<MonoInstaller>(parent);
            foreach (MonoInstaller monoInstaller in children)
            {
                if (installers.FirstOrDefault(installer => installer.GetType() == monoInstaller.GetType()) != null)
                {
                    DestroyImmediate(monoInstaller);
                }
                else
                    installers.Add(monoInstaller);
            }
        }

        private MonoInstaller[] FindDuplicates(Transform parent)
        {
            List<MonoInstaller> installers = new();
            MonoInstaller[] children = AllChildren<MonoInstaller>(parent);
            List<MonoInstaller> duplicates = new();

            foreach (MonoInstaller monoInstaller in children)
            {
                if (installers.FirstOrDefault(installer => installer.GetType() == monoInstaller.GetType()) != null)
                    duplicates.Add(monoInstaller);
                else
                    installers.Add(monoInstaller);
            }

            ;

            return duplicates.ToArray();
        }

        private void DeleteObjectsWithoutInstaller(Transform parent)
        {
            IEnumerable<Transform> children = AllChildren(parent).Where(child => child != null);

            foreach (Transform child in children)
            {
                if (child.TryGetComponent(out MonoInstaller installer) == false)
                    DestroyImmediate(child.gameObject);
            }
        }

        private Transform[] AllChildren(Transform parent)
        {
            List<Transform> children = new();


            foreach (Transform child in parent)
            {
                children.Add(child);
                children.AddRange(AllChildren(child));
            }

            return children.ToArray();
        }

        private T[] AllChildren<T>(Transform parent) where T : Component
        {
            List<T> components = new();

            foreach (Transform child in parent)
            {
                if (child.TryGetComponent(out T component))
                    components.Add(component);

                components.AddRange(AllChildren<T>(child));
            }

            return components.ToArray();
        }

        private string GetFullPath(Transform transform)
        {
            Transform[] parents = transform.GetComponentsInParent<Transform>();

            StringBuilder stringBuilder = new StringBuilder();
            bool contextAppended = false;
            for (int i = parents.Length - 1; i >= 0; i--)
            {
                if (parents[i].name != name && contextAppended == false)
                    continue;
                stringBuilder.Append($"/{parents[i].name}");
                contextAppended = true;
            }

            if (stringBuilder.Length > 0)
                stringBuilder.Remove(0, 1);

            return stringBuilder.ToString();
        }
    }
#endif
}