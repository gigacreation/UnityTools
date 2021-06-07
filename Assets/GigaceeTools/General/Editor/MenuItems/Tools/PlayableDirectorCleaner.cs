using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using static GigaceeTools.MenuItemConstants;

namespace GigaceeTools
{
    public static class PlayableDirectorCleaner
    {
        private const int CategoryPriority = ToolsPriority + 600;
        private const string Category = ToolsDirName + CategoryPrefix + "Clean PlayableDirector" + CategorySuffix;

        [MenuItem(Category, priority = CategoryPriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem(ToolsDirName + "Purge All Playable Directors Bindings", priority = CategoryPriority + 1)]
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
