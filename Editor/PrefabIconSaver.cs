using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
#pragma warning disable CS0162

namespace Editor
{
    [CreateAssetMenu(fileName = nameof(PrefabIconSaver), menuName = nameof(PrefabIconSaver), order = 0)]
    public class PrefabIconSaver : ScriptableObject
    {
        [SerializeField] private List<GameObject> _prefabs;
        [SerializeField] private string _path;
        [SerializeField] private int _width = 1024;
        [SerializeField] private int _height = 1024;

        [Button]
        public void SavePrefabsIcons()
        {
            foreach (GameObject prefab in _prefabs)
            {
                Texture2D prefabPreview = ComputePrefabPreview(prefab);
                SaveTextureAsPNG(prefabPreview, _path, prefab.name);
            }
        }

        private Texture2D ComputePrefabPreview(GameObject prefab)
        {
#if UNITY_EDITOR
            UnityEditor.Editor editor = UnityEditor.Editor.CreateEditor(prefab);
            string prefabPath = AssetDatabase.GetAssetPath(prefab);
            Texture2D texture = editor.RenderStaticPreview(prefabPath, null, _width, _height);
            DestroyImmediate(editor);
            return texture;
#endif
            return null;
        }

        private void SaveTextureAsPNG(Texture2D texture, string path, string name)
        {
            byte[] bytes = texture.EncodeToPNG();
            string fullPath = path + "\\" + name + ".png";
            File.WriteAllBytes(fullPath, bytes);
            Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + fullPath);
        }
    }
}