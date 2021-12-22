// Original code from https://baba-s.hatenablog.com/entry/2020/03/19/031000

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;
using UnityEngine.ResourceManagement.Util;
using static GigaceeTools.MenuItemConstants;
using Object = UnityEngine.Object;

namespace GigaceeTools
{
    public static class AddressablesToolsMenuItems
    {
        private const int CategoryPriority = ToolsPriority + 1000;
        private const string Category = ToolsDirName + CategoryPrefix + "Addressables" + CategorySuffix;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ToolsDirName + "Clear All Cached AssetBundles and Catalog", priority = CategoryPriority + 1)]
        public static void ClearAllCachedAssetBundlesAndCatalog()
        {
            AddressablesTools.ClearAllCachedAssetBundlesAndCatalog();
        }

        [MenuItem(ToolsDirName + "Sort Addressables Groups", priority = CategoryPriority + 2)]
        public static void SortAddressablesGroups()
        {
            // グループを名前でソートします
            AddressablesToolsEditor.SortGroups();

            // Addressable Assets Window の描画を更新します
            AddressablesToolsEditor.RepaintAddressableAssetsWindow();
        }
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class AddressablesToolsEditor
    {
        private static AddressableAssetSettings s_settings;

        private static readonly string[] s_defaultGroupNames = { "Built In Data", "Default Local Group" };

        // Addressable Asset Settings を取得します
        public static AddressableAssetSettings GetSettings()
        {
            if (s_settings != null)
            {
                return s_settings;
            }

            string[] guidList = AssetDatabase.FindAssets("t:AddressableAssetSettings");
            string guid = guidList.FirstOrDefault();
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var settings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(path);

            s_settings = settings;

            return settings;
        }

        // 指定された名前のグループを取得もしくは作成します
        public static AddressableAssetGroup GetOrCreateGroup(string groupName)
        {
            AddressableAssetSettings settings = GetSettings();
            List<AddressableAssetGroup> groups = settings.groups;
            AddressableAssetGroup group = groups.Find(c => c.name == groupName);

            // 既に指定された名前のグループが存在する場合は
            // そのグループを返します
            if (group != null)
            {
                return group;
            }

            // Content Packing & Loading
            var bundleAssetGroupSchema = ScriptableObject.CreateInstance<BundledAssetGroupSchema>();

            // Content Update Restriction
            var contentUpdateGroupSchema = ScriptableObject.CreateInstance<ContentUpdateGroupSchema>();

            // AddressableAssetGroup の Inspector に表示されている Schema
            var schemas = new List<AddressableAssetGroupSchema>
            {
                bundleAssetGroupSchema,
                contentUpdateGroupSchema
            };

            // 指定された名前のグループを作成して返します
            return settings.CreateGroup(groupName, false, false, true, schemas);
        }

        // 指定されたアセットにアドレスを割り当ててグループに追加します
        public static void SetAddressToAssetOrFolder(string path, string address, string groupName)
        {
            AddressableAssetGroup targetParent = GetOrCreateGroup(groupName);

            SetAddressToAssetOrFolder(path, address, targetParent);
        }

        // 指定されたアセットにアドレスを割り当ててグループに追加します
        public static void SetAddressToAssetOrFolder(string path, string address, AddressableAssetGroup targetParent)
        {
            AddressableAssetSettings settings = GetSettings();
            string guid = AssetDatabase.AssetPathToGUID(path);

            AddressableAssetEntry entry = settings.FindAssetEntry(guid);

            // すでに同じアドレスとグループが設定されている場合処理をスキップします
            if ((entry != null) && (entry.address == address) && (entry.parentGroup == targetParent))
            {
                return;
            }

            // アセットをグループに追加します
            entry = settings.CreateOrMoveEntry(guid, targetParent);

            if (entry == null)
            {
                var sb = new StringBuilder();
                sb.AppendLine("[AddressableUtils] Failed AddressableAssetSettings.CreateOrMoveEntry.");
                sb.AppendLine($"path: {path}");
                sb.AppendLine($"address: {address}");
                sb.AppendLine($"targetParent: {targetParent.Name}");
                Debug.LogError(sb.ToString());

                return;
            }

            // アセットにアドレスを割り当てます
            entry.SetAddress(address);
        }

        // 指定されたアセットにラベルを割り当てます
        public static void SetLabelToAssetOrFolder(string path, string label, bool enable)
        {
            AddressableAssetSettings settings = GetSettings();
            string guid = AssetDatabase.AssetPathToGUID(path);
            AddressableAssetEntry entry = settings.FindAssetEntry(guid);

            // アセットにラベルを割り当てます
            // enable が false なら解除します
            // force に true を渡すと、Settings に存在しないラベルが指定された場合、
            // Settings に自動でラベルが追加されます
            entry?.SetLabel(label, enable, true);
        }

        // デフォルト以外のすべてのグループを削除します
        public static void RemoveAllGroupWithoutDefault()
        {
            AddressableAssetSettings settings = GetSettings();
            List<AddressableAssetGroup> groups = settings.groups;

            for (int i = groups.Count - 1; 0 <= i; i--)
            {
                AddressableAssetGroup group = groups[i];

                if (group.IsDefaultGroup())
                {
                    continue;
                }

                settings.RemoveGroup(group);
            }
        }

        // デフォルト以外のすべての空のグループを削除します
        public static void RemoveAllEmptyGroupWithoutDefault()
        {
            AddressableAssetSettings settings = GetSettings();
            List<AddressableAssetGroup> groups = settings.groups;

            for (int i = groups.Count - 1; 0 <= i; i--)
            {
                AddressableAssetGroup group = groups[i];

                if (group.IsDefaultGroup())
                {
                    continue;
                }

                if (0 < group.entries.Count)
                {
                    continue;
                }

                settings.RemoveGroup(group);
            }
        }

        // Play Mode Script のインデックスを設定します
        public static void SetActivePlayModeDataBuilderIndex(int index)
        {
            AddressableAssetSettings settings = GetSettings();
            settings.ActivePlayModeDataBuilderIndex = index;
        }

        // アドレスが設定されているすべてのアセットを返します
        public static IEnumerable<AddressableAssetEntry> GetAllAssets()
        {
            AddressableAssetSettings settings = GetSettings();
            var assets = new List<AddressableAssetEntry>();

            settings.GetAllAssets(assets, true);

            return assets;
        }

        // 重複しているすべてのアドレスを返します
        public static IEnumerable<string> GetDuplicateAddress()
        {
            return GetAllAssets()
                .GroupBy(c => c.address)
                .Where(c => 1 < c.Count())
                .Select(c => c.Key);
        }

        /// <summary>
        /// グループを名前でソートします。
        /// </summary>
        public static void SortGroups()
        {
            AddressableAssetSettings settings = GetSettings();
            List<AddressableAssetGroup> groups = settings.groups;

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

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }

        // AddressableAssetsWindow の描画を更新します
        public static void RepaintAddressableAssetsWindow()
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

        // すべてのキャッシュをクリアします
        // ビルドしたアセットバンドルのキャッシュを削除する関数は ClearBuildCache です
        public static bool ClearDownloadCache()
        {
            return Caching.ClearCache();
        }

        // すべてのキャッシュをクリアします
        // ビルドしたアセットバンドルのキャッシュを削除する関数は ClearBuildCache です
        public static bool ClearDownloadCache(int expiration)
        {
            return Caching.ClearCache(expiration);
        }

        // 従来のアセットバンドルの仕組みで使用するアセットバンドル名をすべて削除します
        public static void RemoveAllAssetBundleName()
        {
            foreach (string n in AssetDatabase.GetAllAssetBundleNames())
            {
                AssetDatabase.RemoveAssetBundleName(n, true);
            }
        }

        // 指定された Schema をすべてのグループから取得します
        public static IEnumerable<T> GetAllSchemas<T>() where T : AddressableAssetGroupSchema
        {
            AddressableAssetSettings settings = GetSettings();
            List<AddressableAssetGroup> groups = settings.groups;

            IEnumerable<T> schemas = groups
                .SelectMany(c => c.Schemas)
                .OfType<T>();

            return schemas;
        }

        // 指定された BundledAssetGroupSchema の AssetBundle Provider を設定します
        public static void SetAssetBundleProviderType(this BundledAssetGroupSchema schema, Type assetBundleProviderType)
        {
            const string Name = "m_AssetBundleProviderType";
            const BindingFlags Attr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var serializedType = new SerializedType
            {
                Value = assetBundleProviderType,
                ValueChanged = true
            };

            Type type = typeof(BundledAssetGroupSchema);
            FieldInfo value = type.GetField(Name, Attr);

            value?.SetValue(schema, serializedType);
        }

        // アセットバンドルをビルドします
        public static void Build()
        {
            AddressableAssetSettings.BuildPlayerContent();
        }

        // アセットバンドルをクリーンビルドします
        public static void CleanBuild()
        {
            ClearBuildCache();
            Build();
        }

        // ビルドしたアセットバンドルのキャッシュを削除します
        // ダウンロードしたアセットバンドルのキャッシュを削除する関数は ClearDownloadCache です
        public static void ClearBuildCache()
        {
            AddressableAssetSettings.CleanPlayerContent();
            BuildCache.PurgeCache(false);
        }

        // すべてのグループ名を返します
        public static IEnumerable<string> GetAllGroupName()
        {
            AddressableAssetSettings settings = GetSettings();
            IEnumerable<string> groupNames = settings.groups.Select(c => c.Name);

            return groupNames;
        }

        // すべてのアドレスを返します
        public static IEnumerable<string> GetAllAddress()
        {
            var regex = new Regex(@"(.*)\[.*\]");

            IEnumerable<string> addresses = GetAllAssets()
                .Select(c => c.address)

                // アドレスの一覧にスプライトも含まれてしまうので
                // スプライト（アドレスの末尾に[]が付くもの）は除外する
                .Select(c => regex.Replace(c, "$1"))

                // 重複しているアドレスも1つにまとめる
                // 重複しているアドレスを知りたい場合は
                // GetDuplicateAddress を使用する
                .GroupBy(c => c)
                .Select(c => c.Key);

            return addresses;
        }

        // ダウンロードしたアセットバンドルのキャッシュが保存されるフォルダのパスを返します
        public static string GetDownloadCacheFolderPath()
        {
            const Environment.SpecialFolder FolderType = Environment.SpecialFolder.LocalApplicationData;

            string folderPath = Environment.GetFolderPath(FolderType);
            string companyName = Application.companyName;
            string productName = Application.productName;
            var path = $"{folderPath}Low/Unity/{companyName}_{productName}";

            path = path.Replace("/", "\\");

            return path;
        }

        // すべてのラベルを返します
        public static IEnumerable<string> GetAllLabel()
        {
            AddressableAssetSettings settings = GetSettings();
            var so = new SerializedObject(settings);
            SerializedProperty labelTable = so.FindProperty("m_LabelTable");
            SerializedProperty labelNames = labelTable.FindPropertyRelative("m_LabelNames");

            for (var i = 0; i < labelNames.arraySize; i++)
            {
                SerializedProperty labelName = labelNames.GetArrayElementAtIndex(i);
                yield return labelName.stringValue;
            }
        }
    }
}
