using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.Editor
{
    [CustomEditor(typeof(BuildTimestampPreference))]
    public class BuildTimestampDisplayEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is not BuildTimestampPreference buildTimestampDisplay)
            {
                return;
            }

            EditorGUILayout.Space();

            if (EditorApplication.isPlaying)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("Set BuildTimestamp Asset", GUILayout.Height(24f)))
            {
                buildTimestampDisplay.SetBuildTimestampAsset();
            }
        }
    }
}
