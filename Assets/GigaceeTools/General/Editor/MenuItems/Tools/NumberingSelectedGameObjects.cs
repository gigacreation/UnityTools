using UnityEditor;
using UnityEngine;
using static GigaceeTools.ToolsMenuItemConstants;

namespace GigaceeTools
{
    public static class NumberingSelectedGameObjects
    {
        private const int CategoryPriority = BasePriority + 500;
        private const string Category = BasePath + CategoryPrefix + "Numbering GameObjects" + CategorySuffix;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(BasePath + "Numbering Selected GameObjects (1)", priority = CategoryPriority + 1)]
        private static void AddNumber0()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                int sibling = go.transform.GetSiblingIndex();
                go.name = $"{go.name}{sibling + 1:D}";
            }
        }

        [MenuItem(BasePath + "Numbering Selected GameObjects (01)", priority = CategoryPriority + 2)]
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
