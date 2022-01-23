using System.Linq;
using UnityEditor;
using UnityEngine;
using static GigaceeTools.ToolsMenuItemConstants;

namespace GigaceeTools
{
    public static class BatchActivator
    {
        private const int CategoryPriority = BasePriority + 300;
        private const string Category = BasePath + CategoryPrefix + "Activate GameObjects" + CategorySuffix;
        private const string ActivateGameObjects = BasePath + "Activate Selected GameObjects And Descendants";

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ActivateGameObjects, priority = CategoryPriority + 1)]
        public static void ActivateSelectedGameObjectsAndDescendants()
        {
            foreach (Transform selection in Selection.transforms)
            {
                foreach (Transform t in selection.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.SetActive(true);
                }
            }
        }

        [MenuItem(ActivateGameObjects, true)]
        private static bool NoSelection()
        {
            return Selection.transforms.Any();
        }
    }
}
