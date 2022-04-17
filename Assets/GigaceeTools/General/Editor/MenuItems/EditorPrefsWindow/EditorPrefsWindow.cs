// Original code from https://github.com/baba-s/UniEditorPrefsWindow

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using UnityEditor;
using UnityEngine;

namespace GigaceeTools
{
    /// <summary>
    /// EditorPrefs が保存しているすべてのキーと値を閲覧できるエディタ拡張
    /// </summary>
    public class EditorPrefsWindow : EditorWindow
    {
        private const int CategoryPriority = 2000001001;
        private const string Category = "Tools/Gigacee Tools/Custom Window/";

        private const float Space = 5f;

        private KeyValueData[] _list;
        private Vector2 _scrollPosition;
        private string _searchText = string.Empty;

        private GUIStyle _windowPadding;

        /// <summary>
        /// 有効になった時に呼び出されます
        /// </summary>
        private void OnEnable()
        {
            Refresh();
        }

        /// <summary>
        /// GUI を描画する時に呼び出されます
        /// </summary>
        private void OnGUI()
        {
            _windowPadding ??= new GUIStyle
            {
                padding = new RectOffset(10, 10, 10, 10)
            };

            using (new EditorGUILayout.VerticalScope(_windowPadding))
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
                {
                    if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
                    {
                        Refresh();
                        Repaint();
                    }

                    if (GUILayout.Button("Delete All", EditorStyles.toolbarButton))
                    {
                        if (EditorUtility.DisplayDialog(
                                "Delete all editor preferences.",
                                "Are you sure you want to delete all the editor preferences? This action cannot be undone.",
                                "Yes",
                                "No"
                            ))
                        {
                            EditorPrefs.DeleteAll();
                        }
                    }

                    GUILayout.FlexibleSpace();

                    _searchText = EditorGUILayout.TextField
                    (
                        _searchText,
                        EditorStyles.toolbarSearchField,
                        GUILayout.Width(256)
                    );
                }

                GUILayout.Space(Space);

                using (var scope = new EditorGUILayout.ScrollViewScope(_scrollPosition))
                {
                    bool isSearch = !string.IsNullOrWhiteSpace(_searchText);
                    IEnumerable<KeyValueData> list = _list.Where(x => !isSearch || x.IsFilter(_searchText));

                    foreach ((string key, string value) in list)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.TextField(key, GUILayout.Width(256));
                            EditorGUILayout.TextField(value);
                        }
                    }

                    _scrollPosition = scope.scrollPosition;
                }
            }
        }

        /// <summary>
        /// キーと値の情報を取得し直します
        /// </summary>
        private void Refresh()
        {
            _list = GetEditorPrefsKeyValuePairAll()
                .OrderBy(x => x.Key)
                .ToArray();
        }

        /// <summary>
        /// 開きます
        /// </summary>
        [MenuItem(Category + "Editor Prefs Window", priority = CategoryPriority)]
        private static void Open()
        {
            GetWindow<EditorPrefsWindow>("Editor Prefs Window");
        }

        /// <summary>
        /// EditorPrefs が保存しているすべてのキーと値の情報を返します
        /// </summary>
        private static IEnumerable<KeyValueData> GetEditorPrefsKeyValuePairAll()
        {
            const string Name = @"Software\Unity Technologies\Unity Editor 5.x\";

            using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Name, false);

            if (registryKey == null)
            {
                yield break;
            }

            foreach (string valueName in registryKey.GetValueNames())
            {
                object value = registryKey.GetValue(valueName);
                string key = valueName.Split(new[] { "_h" }, StringSplitOptions.None)[0];

                if (value is byte[] byteValue)
                {
                    yield return new KeyValueData(key, Encoding.UTF8.GetString(byteValue));
                }
                else
                {
                    yield return new KeyValueData(key, value.ToString());
                }
            }
        }

        /// <summary>
        /// キーと値の情報を管理するクラス
        /// </summary>
        private class KeyValueData
        {
            public KeyValueData(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; }
            public string Value { get; }

            public bool IsFilter(string searchText)
            {
                searchText = searchText.ToLower();

                return Key.ToLower().Contains(searchText) || Value.ToLower().Contains(searchText);
            }

            public void Deconstruct(out string key, out string value)
            {
                (key, value) = (Key, Value);
            }
        }
    }
}
