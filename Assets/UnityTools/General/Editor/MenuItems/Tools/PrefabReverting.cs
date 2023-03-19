using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.General.Editor
{
    public static class PrefabReverting
    {
        private const int CategoryPriority = 29000;
        private const string Category = "Tools/GIGA CREATION/Revert Prefabs/";

        private const InteractionMode U = InteractionMode.UserAction;

        [MenuItem(Category + "Revert Name on Selected Prefabs", priority = CategoryPriority)]
        public static void RevertNameOnSelectedPrefabs()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                var so = new SerializedObject(go);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_Name"), U);
            }
        }

        [MenuItem(Category + "Revert Transform on Selected Prefabs", priority = CategoryPriority + 1)]
        public static void RevertTransformOnSelectedPrefabs()
        {
            foreach (Transform t in Selection.gameObjects.Select(obj => obj.transform))
            {
                var so = new SerializedObject(t);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_LocalPosition"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_LocalRotation"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_LocalScale"), U);
            }
        }

        [MenuItem(Category + "Revert RectTransform on Selected Prefabs", priority = CategoryPriority + 2)]
        public static void RevertRectTransformOnSelectedPrefabs()
        {
            foreach (RectTransform rt in Selection.gameObjects.Select(obj => obj.transform as RectTransform))
            {
                var so = new SerializedObject(rt);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_LocalPosition"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_LocalRotation"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_LocalScale"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_AnchorMin"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_AnchorMax"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_AnchoredPosition"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_SizeDelta"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_Pivot"), U);
            }
        }

        [MenuItem(Category + "Revert BoxCollider2D on Selected Prefabs", priority = CategoryPriority + 3)]
        public static void RevertBoxCollider2DOnSelectedPrefabs()
        {
            foreach (BoxCollider2D c in Selection.gameObjects.Select(obj => obj.GetComponent<BoxCollider2D>()))
            {
                var so = new SerializedObject(c);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_Density"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_Material"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_IsTrigger"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_UsedByEffector"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_UsedByComposite"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_AutoTiling"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_Offset"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_Size"), U);
                PrefabUtility.RevertPropertyOverride(so.FindProperty("m_EdgeRadius"), U);
            }
        }

        [MenuItem(Category + "Revert All Properties on Selected Prefabs", priority = CategoryPriority + 10)]
        public static void RevertAllPropertiesOnSelectedPrefabs()
        {
            foreach (GameObject go in Selection.gameObjects)
            {
                PrefabUtility.RevertPrefabInstance(go, U);
            }
        }
    }
}
