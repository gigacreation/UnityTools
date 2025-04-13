using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays.BuildTimestamps.Editor
{
    [CustomEditor(typeof(DebugTextPreferenceBuildTimestamp))]
    public class DebugTextPreferenceBuildTimestampEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is not DebugTextPreferenceBuildTimestamp buildTimestampDisplay)
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
