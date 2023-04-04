using System.Text;
using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.General.Editor
{
    public static class GameObjectPathUtility
    {
        private const int CategoryPriority = 2000000000;
        private const string Category = "Assets/GIGA CREATION/";
        private const string CopyGameObjectPathName = Category + "Copy Path";

        [MenuItem(CopyGameObjectPathName, priority = CategoryPriority)]
        private static void CopyGameObjectPath()
        {
            GameObject selectedGameObject = Selection.activeGameObject;

            if (selectedGameObject == null)
            {
                return;
            }

            var builder = new StringBuilder(selectedGameObject.transform.name);
            Transform current = selectedGameObject.transform.parent;

            while (current != null)
            {
                builder.Insert(0, current.name + "/");
                current = current.parent;
            }

            GUIUtility.systemCopyBuffer = builder.ToString();
        }
    }
}
