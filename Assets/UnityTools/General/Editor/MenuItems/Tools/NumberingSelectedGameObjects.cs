using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.General.Editor
{
    public static class NumberingSelectedGameObjects
    {
        private const int CategoryPriority = 20002;
        private const string Category = "Tools/GIGA CREATION/Numbering GameObjects/";

        [MenuItem(Category + "Numbering Selected GameObjects (1)", priority = CategoryPriority)]
        private static void AddNumber0()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                int sibling = go.transform.GetSiblingIndex();
                go.name = $"{go.name}{sibling + 1:D}";
            }
        }

        [MenuItem(Category + "Numbering Selected GameObjects (01)", priority = CategoryPriority + 1)]
        private static void AddNumber00()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                int sibling = go.transform.GetSiblingIndex();
                go.name = $"{go.name}{sibling + 1:D2}";
            }
        }
    }
}
