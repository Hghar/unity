using System;
using UnityEngine;
using Space = Realization.TutorialRealization.Commands.RenderSpace;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        public static Space RenderSpace(this GameObject gameObject)
        {
            Canvas canvas = gameObject.GetComponentInParent<Canvas>();
            if (canvas == null)
                return Space.World;

            return canvas.renderMode switch
            {
                RenderMode.WorldSpace => Space.World,
                RenderMode.ScreenSpaceCamera => Space.CanvasCamera,
                RenderMode.ScreenSpaceOverlay => Space.CanvasOverlay,
                _ => throw new Exception()
            };
        }

        public static string Path(this GameObject gameObject)
        {
            string path = gameObject.name;
            if (gameObject.transform.parent != null)
            {
                return Parent(gameObject, path);
            }

            return path;
        }

        private static string Parent(GameObject gameObject, string path)
        {
            if (gameObject.transform.parent == null) return path;

            gameObject = gameObject.transform.parent.gameObject;
            string newPath = gameObject.name + "/" + path;
            return Parent(gameObject, newPath);
        }
    }
}