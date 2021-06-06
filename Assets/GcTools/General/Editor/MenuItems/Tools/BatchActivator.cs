using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GcTools
{
    public static class BatchActivator
    {
        private const int BasePriority = -31700;
        private const string Category = "Tools/GC Tools/-------- Activate GameObjects --------";
        private const string ActivateGameobjects = "Tools/GC Tools/Activate Selected GameObjects And Descendants";

        [MenuItem(Category, priority = BasePriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ActivateGameobjects, priority = BasePriority + 1)]
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
