using System.Linq;
using UnityEditor;
using UnityEngine;
using static GcTools.MenuItemConstants;

namespace GcTools
{
    public static class BatchActivator
    {
        private const int CategoryPriority = ToolsPriority + 300;
        private const string Category = ToolsDirName + CategoryPrefix + "Activate GameObjects" + CategorySuffix;
        private const string ActivateGameobjects = ToolsDirName + "Activate Selected GameObjects And Descendants";

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ActivateGameobjects, priority = CategoryPriority + 1)]
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

        [MenuItem(ActivateGameobjects, true)]
        private static bool NoSelection()
        {
            return Selection.transforms.Any();
        }
    }
}
