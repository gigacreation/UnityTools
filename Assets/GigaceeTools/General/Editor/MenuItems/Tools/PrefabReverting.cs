using System.Linq;
using UnityEditor;
using UnityEngine;
using static GigaceeTools.ToolsMenuItemConstants;

namespace GigaceeTools
{
    public static class PrefabReverting
    {
        private const int CategoryPriority = BasePriority + 100;
        private const string Category = BasePath + CategoryPrefix + "Revert Prefabs" + CategorySuffix;
        private const string RevertName = BasePath + "Revert Name On Selected Prefabs";
        private const string RevertTransform = BasePath + "Revert Transform On Selected Prefabs";
        private const string RevertRectTransform = BasePath + "Revert RectTransform On Selected Prefabs";
        private const string RevertBoxCollider2D = BasePath + "Revert BoxCollider2D On Selected Prefabs";
        private const string RevertAll = BasePath + "Revert All Properties On Selected Prefabs";

        private const InteractionMode U = InteractionMode.UserAction;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(RevertName, priority = CategoryPriority + 1)]
        public static void RevertNameOnSelectedPrefabs()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                var s = new SerializedObject(go);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Name"), U);
            }
        }

        [MenuItem(RevertTransform, priority = CategoryPriority + 2)]
        public static void RevertTransformOnSelectedPrefabs()
        {
            foreach (Transform t in Selection.gameObjects.Select(obj => obj.transform))
            {
                var s = new SerializedObject(t);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalPosition"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalRotation"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalScale"), U);
            }
        }

        [MenuItem(RevertRectTransform, priority = CategoryPriority + 3)]
        public static void RevertRectTransformOnSelectedPrefabs()
        {
            foreach (RectTransform rt in Selection.gameObjects.Select(obj => obj.transform as RectTransform))
            {
                var s = new SerializedObject(rt);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalPosition"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalRotation"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalScale"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_AnchorMin"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_AnchorMax"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_AnchoredPosition"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_SizeDelta"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Pivot"), U);
            }
        }

        [MenuItem(RevertBoxCollider2D, priority = CategoryPriority + 4)]
        public static void RevertBoxCollider2DOnSelectedPrefabs()
        {
            foreach (BoxCollider2D c in Selection.gameObjects.Select(obj => obj.GetComponent<BoxCollider2D>()))
            {
                var s = new SerializedObject(c);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Density"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Material"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_IsTrigger"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_UsedByEffector"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_UsedByComposite"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_AutoTiling"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Offset"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Size"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_EdgeRadius"), U);
            }
        }

        [MenuItem(RevertAll, priority = CategoryPriority + 10)]
        public static void RevertAllPropertiesOnSelectedPrefabs()
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                PrefabUtility.RevertPrefabInstance(obj, U);
            }
        }
    }
}
