using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using static GigaceeTools.ToolsMenuItemConstants;

namespace GigaceeTools
{
    public static class PlayableDirectorCleaner
    {
        private const int CategoryPriority = BasePriority + 600;
        private const string Category = BasePath + CategoryPrefix + "Clean PlayableDirector" + CategorySuffix;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(BasePath + "Purge All Playable Directors Bindings", priority = CategoryPriority + 1)]
        public static void PurgeAllPlayableDirectorsBindings()
        {
            foreach (PlayableDirector director in Object.FindObjectsOfType<PlayableDirector>())
            {
                var so = new SerializedObject(director);
                SerializedProperty sceneBindings = so.FindProperty("m_SceneBindings");

                for (int i = sceneBindings.arraySize - 1; i >= 0; i--)
                {
                    if (!sceneBindings.GetArrayElementAtIndex(i).FindPropertyRelative("key").objectReferenceValue)
                    {
                        sceneBindings.DeleteArrayElementAtIndex(i);
                    }
                }

                so.ApplyModifiedProperties();
            }
        }
    }
}
