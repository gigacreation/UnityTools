using UnityEditor;
using UnityEngine;

namespace GcTools
{
    public static class NumberingSelectedGameObjects
    {
        private const int BasePriority = -2099999500;
        private const string Category = "Tools/GC Tools/-------- Numbering GameObjects --------";

        [MenuItem(Category, priority = BasePriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem("Tools/GC Tools/Numbering Selected GameObjects (1)", priority = BasePriority + 1)]
        private static void AddNumber0()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                int sibling = go.transform.GetSiblingIndex();
                go.name = $"{go.name}{sibling + 1:D}";
            }
        }

        [MenuItem("Tools/GC Tools/Numbering Selected GameObjects (01)", priority = BasePriority + 2)]
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
