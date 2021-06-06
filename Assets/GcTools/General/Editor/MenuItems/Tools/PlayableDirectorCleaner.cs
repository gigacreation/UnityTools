using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace GcTools
{
    public static class PlayableDirectorCleaner
    {
        private const int BasePriority = -2099999400;
        private const string Category = "Tools/GC Tools/-------- Clean PlayableDirector --------";

        [MenuItem(Category, priority = BasePriority)]
        public static void CategoryName()
        {
        }

        [MenuItem(Category, true)]
        private static bool CategoryValidate()
        {
            return false;
        }

        [MenuItem("Tools/GC Tools/Purge All Playable Directors Bindings", priority = BasePriority + 1)]
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
