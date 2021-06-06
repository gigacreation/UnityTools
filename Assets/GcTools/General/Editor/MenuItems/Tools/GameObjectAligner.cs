using System.Linq;
using UnityEditor;
using UnityEngine;
using static GcTools.MenuItemConstants;

namespace GcTools
{
    public static class GameObjectAligner
    {
        private const int CategoryPriority = ToolsPriority + 200;
        private const string Category = ToolsDirName + CategoryPrefix + "Align GameObjects" + CategorySuffix;
        private const string Position100 = "Tools/GC Tools/Align Selected GameObject (1.00)";
        private const string Position050 = "Tools/GC Tools/Align Selected GameObject (0.50)";
        private const string Position025 = "Tools/GC Tools/Align Selected GameObject (0.25)";

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(Position100, priority = CategoryPriority + 1)]
        public static void MakePositionSharper1_00()
        {
            foreach (GameObject selection in Selection.gameObjects)
            {
                Vector3 vec3 = selection.transform.position;
                vec3.Set(Mathf.Round(vec3.x), Mathf.Round(vec3.y), Mathf.Round(vec3.z));
                selection.transform.position = vec3;
            }
        }

        [MenuItem(Position050, priority = CategoryPriority + 2)]
        public static void MakePositionSharper0_50()
        {
            foreach (GameObject selection in Selection.gameObjects)
            {
                Vector3 vec3 = selection.transform.position;
                vec3.Set(Mathf.Round(vec3.x * 2f) / 2f, Mathf.Round(vec3.y * 2f) / 2f, Mathf.Round(vec3.z * 2f) / 2f);
                selection.transform.position = vec3;
            }
        }

        [MenuItem(Position025, priority = CategoryPriority + 3)]
        public static void MakePositionSharper0_25()
        {
            foreach (GameObject selection in Selection.gameObjects)
            {
                Vector3 vec3 = selection.transform.position;
                vec3.Set(Mathf.Round(vec3.x * 4f) / 4f, Mathf.Round(vec3.y * 4f) / 4f, Mathf.Round(vec3.z * 4f) / 4f);
                selection.transform.position = vec3;
            }
        }

        [MenuItem(Position100, true), MenuItem(Position050, true), MenuItem(Position025, true)]
        private static bool NoSelection()
        {
            return Selection.transforms.Any();
        }
    }
}
