using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace GigaceeTools
{
    public static class GigaceeToolsShortcuts
    {
        private const int CategoryPriority = 20999;
        private const string Category = "Tools/Gigacee Tools/Shortcuts/";

        [MenuItem(Category + "Check if Root Prefabs have Changed", priority = CategoryPriority)]
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

            if (!overriddenPrefabInstances.Any())
            {
                Debug.Log("ルートに変更された Prefab はありませんでした。");
            }

            // 変更されている Prefab Instance が存在していたら、それらを選択する
            Selection.objects = overriddenPrefabInstances;
        }

        [MenuItem(Category + "Restore Rainbow Folders", priority = CategoryPriority + 1)]
        public static async Task RestoreRainbowFolders()
        {
            // UnityEditor.dllを取得する
            Assembly asm = Assembly.Load("UnityEditor");

            // ProjectBrowserクラスを取得する
            Type projectWindowType = asm.GetType("UnityEditor.ProjectBrowser");

            // 列挙体 ProjectBrowser.ViewMode を取得する
            Type viewModeType = asm.GetType("UnityEditor.ProjectBrowser+ViewMode");

            // ビューモードを設定するメソッドを取得する
            MethodInfo initViewMode = projectWindowType.GetMethod(
                "InitViewMode",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static
            );

            // プロジェクトウィンドウを取得する
            EditorWindow projectWindow = EditorWindow.GetWindow(projectWindowType, false, "Project", false);

            // プロジェクトウィンドウにフォーカスする
            projectWindow.Focus();

            // プロジェクトウィンドウを 1 カラム表示に変更する
            initViewMode?.Invoke(projectWindow, new[] { Enum.GetValues(viewModeType).GetValue(0) });

            // 少し待つ
            await Task.Delay(100);

            // プロジェクトウィンドウを 2 カラム表示に変更する
            initViewMode?.Invoke(projectWindow, new[] { Enum.GetValues(viewModeType).GetValue(1) });
        }

        [MenuItem(Category + "Clear Console", priority = CategoryPriority + 2)]
        public static void ClearConsole()
        {
            Type
                .GetType("UnityEditor.LogEntries, UnityEditor.dll")
                ?.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public)
                ?.Invoke(null, null);
        }
    }
}
