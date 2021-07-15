﻿using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static GigaceeTools.MenuItemConstants;

namespace GigaceeTools
{
    public class TagAndLayerFinder : EditorWindow
    {
        private const int CategoryPriority = ToolsPriority - 100;
        private const string Category = ToolsDirName + CategoryPrefix + "Custom Window" + CategorySuffix;

        private const string CurrentScene = "Current Scene";
        private const string AllEnabledScenes = "All Enabled Scenes";
        private const string Project = "Project";

        private const float SmallSpace = 5f;
        private const float LargeSpace = 20f;

        private GUIStyle _area;
        private GUIStyle _heading;

        private string _inputTag;
        private string _inputTagProjectPath;

        private string _inputSortingLayer;
        private string _inputSortingLayerProjectPath;

        private int _inputLayer;
        private string _inputLayerProjectPath;

        private void OnGUI()
        {
            if (_area == null)
            {
                _area = new GUIStyle
                {
                    padding = new RectOffset(10, 10, 10, 10)
                };
            }

            if (_heading == null)
            {
                _heading = new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold,
                    fontSize = 20
                };
            }

            using (new EditorGUILayout.ScrollViewScope(Vector2.zero))
            {
                using (new EditorGUILayout.VerticalScope(_area))
                {
                    GUILayout.Label("Tag", _heading);
                    GUILayout.Space(SmallSpace);
                    _inputTag = EditorGUILayout.TagField("Tag: ", _inputTag);
                    _inputTagProjectPath = EditorGUILayout.TextField("Project Path: ", _inputTagProjectPath);
                    GUILayout.Space(SmallSpace);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(CurrentScene))
                        {
                            FindGameObjectsWithTagInCurrentScene(_inputTag);
                        }

                        if (GUILayout.Button(AllEnabledScenes))
                        {
                            FindGameObjectsWithTagInAllEnabledScenes(_inputTag);
                        }

                        if (GUILayout.Button(Project))
                        {
                            FindGameObjectsWithTagInProject(_inputTag, _inputTagProjectPath);
                        }
                    }

                    GUILayout.Space(LargeSpace);
                    GUILayout.Label("Sorting Layer", _heading);
                    GUILayout.Space(SmallSpace);
                    _inputSortingLayer = EditorGUILayout.TextField("Sorting Layer: ", _inputSortingLayer);
                    _inputSortingLayerProjectPath =
                        EditorGUILayout.TextField("Project Path: ", _inputSortingLayerProjectPath);
                    GUILayout.Space(SmallSpace);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(CurrentScene))
                        {
                            FindGameObjectsWithSortingLayerInCurrentScene(_inputSortingLayer);
                        }

                        if (GUILayout.Button(AllEnabledScenes))
                        {
                            FindGameObjectsWithSortingLayerInAllEnabledScenes(_inputSortingLayer);
                        }

                        if (GUILayout.Button(Project))
                        {
                            FindGameObjectsWithSortingLayerInProject(_inputSortingLayer, _inputSortingLayerProjectPath);
                        }
                    }

                    GUILayout.Space(LargeSpace);
                    GUILayout.Label("Layer", _heading);
                    GUILayout.Space(SmallSpace);
                    _inputLayer = EditorGUILayout.LayerField("Layer: ", _inputLayer);
                    _inputLayerProjectPath = EditorGUILayout.TextField("Project Path: ", _inputLayerProjectPath);
                    GUILayout.Space(SmallSpace);

                    using (new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(CurrentScene))
                        {
                            FindGameObjectsWithLayerInCurrentScene(_inputLayer);
                        }

                        if (GUILayout.Button(AllEnabledScenes))
                        {
                            FindGameObjectsWithLayerInAllEnabledScenes(_inputLayer);
                        }

                        if (GUILayout.Button(Project))
                        {
                            FindGameObjectsWithLayerInProject(_inputLayer, _inputLayerProjectPath);
                        }
                    }
                }
            }
        }

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ToolsDirName + "Tag and Layer Finder", priority = CategoryPriority + 1)]
        private static void ShowWindow()
        {
            var window = GetWindow<TagAndLayerFinder>();
            window.titleContent = new GUIContent("Tag and Layer Finder");
            window.Show();
        }

        private static void FindGameObjectsWithTagInScene(string tag)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(obj => obj.scene.isLoaded && (obj.hideFlags == HideFlags.None)))
            {
                if (obj.CompareTag(tag))
                {
                    Debug.Log($"{sceneName}/{obj.transform.GetPath()}");
                }
            }
        }

        private static void FindGameObjectsWithTagInCurrentScene(string tag)
        {
            FindGameObjectsWithTagInScene(tag);

            Debug.Log("The tag search is complete.");
        }

        private static void FindGameObjectsWithTagInAllEnabledScenes(string tag)
        {
            string currentScenePath = SceneManager.GetActiveScene().path;

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                EditorSceneManager.OpenScene(scene.path);
                FindGameObjectsWithTagInScene(tag);
            }

            EditorSceneManager.OpenScene(currentScenePath);

            Debug.Log("The tag search is complete.");
        }

        private static void FindGameObjectsWithTagInProject(string tag, string pathToSearch)
        {
            foreach (string assetPath in AssetDatabase.FindAssets("t:Prefab").Select(AssetDatabase.GUIDToAssetPath))
            {
                if (!string.IsNullOrEmpty(pathToSearch) && !assetPath.StartsWith(pathToSearch))
                {
                    continue;
                }

                if (AssetDatabase.LoadAssetAtPath<GameObject>(assetPath).CompareTag(tag))
                {
                    Debug.Log(assetPath);
                }
            }

            Debug.Log("The tag search is complete.");
        }

        private static void FindGameObjectsWithSortingLayerInScene(string sortingLayer)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(obj => obj.scene.isLoaded && (obj.hideFlags == HideFlags.None)))
            {
                if (obj.TryGetComponent(out Renderer renderer))
                {
                    if (renderer.sortingLayerName == sortingLayer)
                    {
                        Debug.Log($"{sceneName}/{obj.transform.GetPath()} (Sorting Order: {renderer.sortingOrder})");
                    }
                }

                if (obj.TryGetComponent(out SortingGroup sortingGroup))
                {
                    if (sortingGroup.sortingLayerName == sortingLayer)
                    {
                        Debug.Log($"{sceneName}/{obj.transform.GetPath()} (Sorting Order: {renderer.sortingOrder})");
                    }
                }

                // ReSharper disable once InvertIf
                if (obj.TryGetComponent(out Canvas canvas))
                {
                    if (canvas.sortingLayerName == sortingLayer)
                    {
                        Debug.Log($"{sceneName}/{obj.transform.GetPath()} (Sorting Order: {renderer.sortingOrder})");
                    }
                }
            }
        }

        private static void FindGameObjectsWithSortingLayerInCurrentScene(string sortingLayer)
        {
            FindGameObjectsWithSortingLayerInScene(sortingLayer);

            Debug.Log("The sorting layer search is complete.");
        }

        private static void FindGameObjectsWithSortingLayerInAllEnabledScenes(string sortingLayer)
        {
            string currentScenePath = SceneManager.GetActiveScene().path;

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                EditorSceneManager.OpenScene(scene.path);
                FindGameObjectsWithSortingLayerInScene(sortingLayer);
            }

            EditorSceneManager.OpenScene(currentScenePath);

            Debug.Log("The sorting layer search is complete.");
        }

        private static void FindGameObjectsWithSortingLayerInProject(string sortingLayer, string pathToSearch)
        {
            foreach (string assetPath in AssetDatabase.FindAssets("t:Prefab").Select(AssetDatabase.GUIDToAssetPath))
            {
                if (!string.IsNullOrEmpty(pathToSearch) && !assetPath.StartsWith(pathToSearch))
                {
                    continue;
                }

                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (!obj)
                {
                    continue;
                }

                if (obj.TryGetComponent(out Renderer renderer))
                {
                    if (renderer.sortingLayerName == sortingLayer)
                    {
                        Debug.Log($"{assetPath} (Sorting Order: {renderer.sortingOrder})");
                    }
                }

                if (obj.TryGetComponent(out SortingGroup sortingGroup))
                {
                    if (sortingGroup.sortingLayerName == sortingLayer)
                    {
                        Debug.Log($"{assetPath} (Sorting Order: {renderer.sortingOrder})");
                    }
                }

                // ReSharper disable once InvertIf
                if (obj.TryGetComponent(out Canvas canvas))
                {
                    if (canvas.sortingLayerName == sortingLayer)
                    {
                        Debug.Log($"{assetPath} (Sorting Order: {renderer.sortingOrder})");
                    }
                }
            }

            Debug.Log("The sorting layer search is complete.");
        }

        private static void FindGameObjectsWithLayerInScene(int layer)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(obj => obj.scene.isLoaded && (obj.hideFlags == HideFlags.None)))
            {
                if (obj.layer == layer)
                {
                    Debug.Log($"{sceneName}/{obj.transform.GetPath()}");
                }
            }
        }

        private static void FindGameObjectsWithLayerInCurrentScene(int layer)
        {
            FindGameObjectsWithLayerInScene(layer);

            Debug.Log("The layer search is complete.");
        }

        private static void FindGameObjectsWithLayerInAllEnabledScenes(int layer)
        {
            string currentScenePath = SceneManager.GetActiveScene().path;

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                EditorSceneManager.OpenScene(scene.path);
                FindGameObjectsWithLayerInScene(layer);
            }

            EditorSceneManager.OpenScene(currentScenePath);

            Debug.Log("The layer search is complete.");
        }

        private static void FindGameObjectsWithLayerInProject(int layer, string pathToSearch)
        {
            foreach (string assetPath in AssetDatabase.FindAssets("t:Prefab").Select(AssetDatabase.GUIDToAssetPath))
            {
                if (!string.IsNullOrEmpty(pathToSearch) && !assetPath.StartsWith(pathToSearch))
                {
                    continue;
                }

                if (AssetDatabase.LoadAssetAtPath<GameObject>(assetPath).layer == layer)
                {
                    Debug.Log(assetPath);
                }
            }

            Debug.Log("The layer search is complete.");
        }
    }

    public static class TagAndLayerFinderExtensions
    {
        public static string GetPath(this Transform self)
        {
            string path = self.gameObject.name;
            Transform parent = self.parent;

            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }

            return path;
        }
    }
}