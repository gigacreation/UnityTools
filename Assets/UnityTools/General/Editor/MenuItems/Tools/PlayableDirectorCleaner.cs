using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace GigaCreation.Tools.General.Editor
{
    public static class PlayableDirectorCleaner
    {
        private const int CategoryPriority = 29003;
        private const string Category = "Tools/GIGA CREATION/Playable Director/";

        [MenuItem(Category + "Purge All Playable Directors Bindings", priority = CategoryPriority)]
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
