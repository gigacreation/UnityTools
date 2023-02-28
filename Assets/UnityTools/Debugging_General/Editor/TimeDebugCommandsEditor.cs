using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools
{
    [CustomEditor(typeof(TimeDebugCommands))]
    public class TimeDebugCommandsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target is TimeDebugCommands timeDebugCommands))
            {
                return;
            }

            EditorGUILayout.HelpBox("上記の修飾キーを押しながら ←↓↑→ を押すと、タイムスケールを変更できます。", MessageType.Info);

            GUILayout.Space(8f);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUI.BeginDisabledGroup(!Application.isPlaying);

                if (GUILayout.Button("<< -0.2", GUILayout.Height(24f)))
                {
                    timeDebugCommands.SlowDown(0.2f);
                }

                if (GUILayout.Button("Pause", GUILayout.Height(24f)))
                {
                    timeDebugCommands.TogglePause();
                }

                if (GUILayout.Button("+0.2 >>", GUILayout.Height(24f)))
                {
                    timeDebugCommands.SpeedUp(0.2f);
                }

                EditorGUI.EndDisabledGroup();
            }
        }
    }
}
