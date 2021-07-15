﻿using System.Linq;
using UnityEditor;
using UnityEngine;
using static GigaceeTools.MenuItemConstants;

namespace GigaceeTools
{
    public static class PrefabReverting
    {
        private const int CategoryPriority = ToolsPriority + 100;
        private const string Category = ToolsDirName + CategoryPrefix + "Revert Prefabs" + CategorySuffix;
        private const string RevertName = ToolsDirName + "Revert Name On Selected Prefabs";
        private const string RevertTransform = ToolsDirName + "Revert Transform On Selected Prefabs";
        private const string RevertRectTransform = ToolsDirName + "Revert RectTransform On Selected Prefabs";
        private const string RevertAll = ToolsDirName + "Revert All Properties On Selected Prefabs";

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
            foreach (GameObject obj in Selection.gameObjects)
            {
                var s = new SerializedObject(obj);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_Name"), U);
            }
        }

        [MenuItem(RevertTransform, priority = CategoryPriority + 2)]
        public static void RevertTransformOnSelectedPrefabs()
        {
            foreach (Transform trans in Selection.gameObjects.Select(obj => obj.transform))
            {
                var s = new SerializedObject(trans);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalPosition"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalRotation"), U);
                PrefabUtility.RevertPropertyOverride(s.FindProperty("m_LocalScale"), U);
            }
        }

        [MenuItem(RevertRectTransform, priority = CategoryPriority + 3)]
        public static void RevertRectTransformOnSelectedPrefabs()
        {
            foreach (RectTransform rect in Selection.gameObjects.Select(obj => obj.transform as RectTransform))
            {
                var s = new SerializedObject(rect);
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