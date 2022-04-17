using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

namespace GigaceeTools
{
    public static class PlayableDirectorCleaner
    {
        private const int CategoryPriority = 2000000020;
        private const string Category = "Tools/Gigacee Tools/Playable Director/";

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
