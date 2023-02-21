using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GigaCreation.Tools
{
    public static class UnityToolsShortcuts
    {
        private const int CategoryPriority = 20999;
        private const string Category = "Tools/GIGA CREATION/Shortcuts/";

        [MenuItem(Category + "Clear Console", priority = CategoryPriority)]
        public static void ClearConsole()
        {
            Type
                .GetType("UnityEditor.LogEntries, UnityEditor.dll")
                ?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)
                ?.Invoke(null, null);
        }

        [MenuItem(Category + "Check if Root Prefabs have Changed", priority = CategoryPriority + 1)]
        public static void CheckIfRootPrefabsHaveChanged()
        {
            IEnumerable<GameObject> rootGameObjects;

            PrefabStage currentPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            if (currentPrefabStage == null)
            {
                // Prefab Mode でない場合、シーンのルートにある GameObject を取得する
                rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            }
            else
            {
                // Prefab Mode の場合、ルートの Prefab の 1 階層下にある GameObject を取得する
                rootGameObjects = currentPrefabStage
                    .prefabContentsRoot
                    .transform
                    .Cast<Transform>()
                    .Select(x => x.gameObject);
            }

            // 変更されている Prefab Instance を抽出する
            Object[] overriddenPrefabInstances = rootGameObjects
                .Where(PrefabUtility.IsAnyPrefabInstanceRoot)
                .Where(x => PrefabUtility.HasPrefabInstanceAnyOverrides(x, false))
                .Select(x => (Object)x)
                .ToArray();

            foreach (Object instance in overriddenPrefabInstances)
            {
                Debug.Log($"ルートの Prefab が変更されています: {instance}");
            }

            if (overriddenPrefabInstances.Length == 0)
            {
                Debug.Log("ルートに変更された Prefab はありませんでした。");
            }

            // 変更されている Prefab Instance が存在していたら、それらを選択する
            Selection.objects = overriddenPrefabInstances;
        }
    }
}
