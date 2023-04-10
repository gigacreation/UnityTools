using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.General.Editor
{
    public static class GameObjectPathUtility
    {
        private const int CategoryPriority = 1000;
        private const string Category = "GameObject/";
        private const string CopyGameObjectPathName = Category + "Copy Path";

        [MenuItem(CopyGameObjectPathName, priority = CategoryPriority)]
        private static void CopyGameObjectPath()
        {
            GameObject[] gameObjects = Selection.objects.OfType<GameObject>().ToArray();

            if (gameObjects.Length == 0)
            {
                return;
            }

            var paths = new string[gameObjects.Length];
            var sb = new StringBuilder();

            for (var i = 0; i < gameObjects.Length; i++)
            {
                sb.Clear();

                sb.Append(gameObjects[i].name);

                Transform current = gameObjects[i].transform.parent;

                while (current != null)
                {
                    sb.Insert(0, current.name + "/");
                    current = current.parent;
                }

                paths[i] = sb.ToString();
            }

            GUIUtility.systemCopyBuffer = string.Join("\n", paths);
        }
    }
}
