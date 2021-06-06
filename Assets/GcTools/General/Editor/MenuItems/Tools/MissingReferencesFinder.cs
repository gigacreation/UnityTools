using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GcTools.MenuItemConstants;

namespace GcTools
{
    public static class MissingReferencesFinder
    {
        private const int CategoryPriority = ToolsPriority;
        private const string Category = ToolsDirName + CategoryPrefix + "Find Missing References" + CategorySuffix;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ToolsDirName + "Find Missing References in Current Scene", priority = CategoryPriority + 1)]
        public static void FindMissingReferencesInCurrentScene()
        {
            FindMissingReferences(SceneManager.GetActiveScene().path, GetSceneObjects());

            Debug.Log($"{SceneManager.GetActiveScene().name}: The process is finished.");
        }

        [MenuItem(ToolsDirName + "Find Missing References in All Enabled Scenes", priority = CategoryPriority + 2)]
        public static void FindMissingReferencesInAllEnabledScenes()
        {
            string currentScenePath = SceneManager.GetActiveScene().path;

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes.Where(s => s.enabled))
            {
                EditorSceneManager.OpenScene(scene.path);
                FindMissingReferencesInCurrentScene();
            }

            EditorSceneManager.OpenScene(currentScenePath);

            Debug.Log("All processes are finished.");
        }

        [MenuItem(ToolsDirName + "Find Missing References in All Scenes", priority = CategoryPriority + 3)]
        public static void FindMissingReferencesInAllScenes()
        {
            string currentScenePath = SceneManager.GetActiveScene().path;

            foreach (string path in AssetDatabase.FindAssets("t:Scene").Select(AssetDatabase.GUIDToAssetPath))
            {
                EditorSceneManager.OpenScene(path);
                FindMissingReferencesInCurrentScene();
            }

            EditorSceneManager.OpenScene(currentScenePath);

            Debug.Log("All processes are finished.");
        }

        [MenuItem(ToolsDirName + "Find Missing References in Assets", priority = CategoryPriority + 4)]
        public static void FindMissingReferencesInAssets()
        {
            foreach (string path in AssetDatabase.FindAssets("t:Prefab").Select(AssetDatabase.GUIDToAssetPath))
            {
                FindMissingReferences(
                    path,
                    AssetDatabase
                        .LoadAssetAtPath<Transform>(path)
                        .GetComponentsInChildren<Transform>()
                        .Select(x => x.gameObject)
                );
            }

            Debug.Log("The process is finished.");
        }

        private static void FindMissingReferences(string context, IEnumerable<GameObject> gameObjects)
        {
            if (gameObjects == null)
            {
                return;
            }

            foreach (GameObject go in gameObjects)
            {
                Component[] components = go.GetComponents<Component>();

                foreach (Component component in components)
                {
                    if (!component)
                    {
                        Debug.LogError($"Missing Component in GameObject: {GetFullPath(go)}", go);
                        continue;
                    }

                    var so = new SerializedObject(component);
                    SerializedProperty sp = so.GetIterator();

                    PropertyInfo objRefValueMethod = typeof(SerializedProperty).GetProperty(
                        "objectReferenceStringValue",
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                    );

                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType != SerializedPropertyType.ObjectReference)
                        {
                            continue;
                        }

                        var objectReferenceStringValue = string.Empty;

                        if (objRefValueMethod != null)
                        {
                            objectReferenceStringValue = (string)objRefValueMethod
                                .GetGetMethod(true)
                                .Invoke(sp, new object[] { });
                        }

                        if ((sp.objectReferenceValue == null)
                            && ((sp.objectReferenceInstanceIDValue != 0)
                                || objectReferenceStringValue.StartsWith("Missing")))
                        {
                            Debug.LogError(
                                $"Missing Ref in {context} - {GetFullPath(go)} - {component.GetType().Name} - "
                                + ObjectNames.NicifyVariableName(sp.name),
                                go
                            );
                        }
                    }
                }
            }
        }

        private static IEnumerable<GameObject> GetSceneObjects()
        {
            // 非アクティブの GameObject も含めるため、GameObject.FindObjectsOfType ではなくこちらを使う
            return Resources.FindObjectsOfTypeAll<GameObject>()
                .Where(obj =>
                    string.IsNullOrEmpty(AssetDatabase.GetAssetPath(obj)) &&
                    (obj.hideFlags == HideFlags.None));
        }

        private static string GetFullPath(GameObject go)
        {
            Transform parent = go.transform.parent;

            return parent == null ? go.name : GetFullPath(parent.gameObject) + "/" + go.name;
        }
    }
}
