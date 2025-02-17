using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
#if ADDRESSABLE_ASSET_GROUP_SORT_SETTINGS_AVAILABLE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using Object = UnityEngine.Object;
#endif

namespace GigaCreation.Tools.Addressables.Editor
{
    public static class AddressablesMenuItems
    {
        private const int CategoryPriority = 29100;
        private const string Category = "Tools/GIGA CREATION/Addressables/";

        private static AddressableAssetSettings s_addressablesSettings;

        private static AddressableAssetSettings AddressablesSettings
            => s_addressablesSettings ??= AddressableAssetSettingsDefaultObject.Settings;

        /// <summary>
        /// Addressables のクリーンビルドを行います。その際、以前のビルドが存在していたら削除します。
        /// </summary>
        [MenuItem(Category + "Full Clean Build", priority = CategoryPriority + 1)]
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

        [MenuItem(Category + "Clear Cached Catalogs and Bundles", priority = CategoryPriority + 2)]
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

#if ADDRESSABLE_ASSET_GROUP_SORT_SETTINGS_AVAILABLE
        private static readonly string[] s_defaultGroupNames = { "Built In Data", "Default Local Group" };

        [MenuItem(Category + "Sort Addressables Groups", priority = CategoryPriority + 3)]
        public static void SortAddressablesGroups()
        {
            // Addressable Assets Window の描画を更新します
            CloseAddressableAssetsWindow();

            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            List<AddressableAssetGroup> groups = settings.groups;
            AddressableAssetGroupSortSettings sortingSettings = AddressableAssetGroupSortSettings.GetSettings();

            groups.Sort(static (a, b) =>
            {
                int indexOfA = Array.IndexOf(s_defaultGroupNames, a.Name);
                int indexOfB = Array.IndexOf(s_defaultGroupNames, b.Name);

                return indexOfA switch
                {
                    // 両方ともデフォルトのグループではない場合
                    -1 when indexOfB == -1 => string.Compare(a.Name, b.Name, StringComparison.Ordinal),
                    // 両方ともデフォルトのグループである場合
                    >= 0 when indexOfB >= 0 => indexOfA - indexOfB,
                    // 片方だけデフォルトのグループである場合
                    _ => indexOfB - indexOfA
                };
            });

            groups.Select(static group => group.Guid).ToArray().CopyTo(sortingSettings.sortOrder, 0);

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        // Original code from https://baba-s.hatenablog.com/entry/2020/03/19/031000
        /// <summary>
        /// Addressables ウィンドウを閉じます。
        /// </summary>
        private static void CloseAddressableAssetsWindow()
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

            window.Close();
        }
#endif
    }
}
