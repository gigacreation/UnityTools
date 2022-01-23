using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using static GigaceeTools.ToolsMenuItemConstants;
using Object = UnityEngine.Object;

namespace GigaceeTools
{
    public static class AddressablesMenuItems
    {
        private const int CategoryPriority = BasePriority + 1000;
        private const string Category = BasePath + CategoryPrefix + "Addressables" + CategorySuffix;
        private const string FullCleanBuildItemName = BasePath + "Full Clean Build";
        private const string ClearCachedCatalogsAndBundlesItemName = BasePath + "Clear Cached Catalogs and Bundles";
        private const string SortAddressablesGroupsItemName = BasePath + "Sort Addressables Groups";

        private static readonly string[] s_defaultGroupNames = { "Built In Data", "Default Local Group" };

        private static AddressableAssetSettings s_addressablesSettings;

        private static AddressableAssetSettings AddressablesSettings
            => s_addressablesSettings ??= AddressableAssetSettingsDefaultObject.Settings;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        /// <summary>
        /// Addressables のクリーンビルドを行います。その際、以前のビルドが存在していたら削除します。
        /// </summary>
        [MenuItem(FullCleanBuildItemName, priority = CategoryPriority + 1)]
        public static void FullCleanBuild()
        {
            string remoteBuildRawPath = AddressablesSettings
                .profileSettings
                .GetValueByName(AddressablesSettings.activeProfileId, AddressableAssetSettings.kRemoteBuildPath);

            string remoteBuildPath = AddressablesSettings
                .profileSettings
                .EvaluateString(AddressablesSettings.activeProfileId, remoteBuildRawPath);

            string remoteBuildFullPath = Path
                .GetFullPath(Path.Combine(Application.dataPath, "../", remoteBuildPath))
                .Replace("\\", "/");

            if (!Directory.Exists(remoteBuildFullPath))
            {
                Debug.Log("Addressables のリモートビルドディレクトリが存在していません。");
            }
            else
            {
                Directory.Delete(remoteBuildFullPath, true);
                Debug.Log("Addressables のリモートビルドディレクトリを削除しました。");
            }

            AddressableAssetSettings.CleanPlayerContent();
            BuildCache.PurgeCache(false);

            AddressableAssetSettings.BuildPlayerContent();
        }

        [MenuItem(ClearCachedCatalogsAndBundlesItemName, priority = CategoryPriority + 2)]
        public static void ClearCachedCatalogsAndBundles()
        {
            string catalogDirPath = Path.Combine(Application.persistentDataPath, "com.unity.addressables");

            if (Directory.Exists(catalogDirPath))
            {
                string[] files = Directory.GetFiles(catalogDirPath, "*", SearchOption.AllDirectories);

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        File.Delete(file);
                    }

                    Debug.Log("コンテンツカタログのキャッシュクリアに成功しました。");
                }
                else
                {
                    Debug.Log("コンテンツカタログのキャッシュディレクトリ内にファイルが存在していません。");
                }
            }
            else
            {
                Debug.Log("コンテンツカタログのキャッシュディレクトリが存在していません。");
            }

            bool success = Caching.ClearCache();

            Debug.Log($"アセットバンドルのキャッシュクリアに{(success ? "成功" : "失敗")}しました。");
        }

        [MenuItem(SortAddressablesGroupsItemName, priority = CategoryPriority + 3)]
        public static void SortAddressablesGroups()
        {
            List<AddressableAssetGroup> groups = AddressablesSettings.groups;

            groups.Sort((a, b) =>
            {
                int indexOfA = Array.IndexOf(s_defaultGroupNames, a.Name);
                int indexOfB = Array.IndexOf(s_defaultGroupNames, b.Name);

                // 両方ともデフォルトのグループではない場合
                if ((indexOfA == -1) && (indexOfB == -1))
                {
                    return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
                }

                // 両方ともデフォルトのグループである場合
                if ((indexOfA >= 0) && (indexOfB >= 0))
                {
                    return indexOfA - indexOfB;
                }

                // 片方だけデフォルトのグループである場合
                return indexOfB - indexOfA;
            });

            EditorUtility.SetDirty(AddressablesSettings);
            AssetDatabase.SaveAssets();

            // Addressable Assets Window の描画を更新します
            RepaintAddressableAssetsWindow();
        }

        // Original code from https://baba-s.hatenablog.com/entry/2020/03/19/031000
        /// <summary>
        /// Addressables ウィンドウの描画を更新します。
        /// </summary>
        private static void RepaintAddressableAssetsWindow()
        {
            const BindingFlags Attr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            const string AssemblyName = "Unity.Addressables.Editor";
            const string WindowTypeName = "UnityEditor.AddressableAssets.GUI.AddressableAssetsWindow";
            const string GroupEditorTypeName = "UnityEditor.AddressableAssets.GUI.AddressableAssetsSettingsGroupEditor";

            Assembly assembly = Assembly.Load(AssemblyName);
            Type windowType = assembly.GetType(WindowTypeName);
            Object[] windows = Resources.FindObjectsOfTypeAll(windowType);
            bool isOpen = 1 <= windows.Length;

            if (!isOpen)
            {
                return;
            }

            var window = windows[0] as EditorWindow;
            Type groupEditorType = assembly.GetType(GroupEditorTypeName);
            FieldInfo groupEditorField = windowType.GetField("m_GroupEditor", Attr);
            MethodInfo method = groupEditorType.GetMethod("Reload", Attr);
            object groupEditor = groupEditorField?.GetValue(window);

            method?.Invoke(groupEditor, Array.Empty<object>());

            if (!window)
            {
                return;
            }

            window.Repaint();
        }
    }
}
