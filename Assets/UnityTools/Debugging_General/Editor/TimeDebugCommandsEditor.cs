using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.General.Editor
{
    [CustomEditor(typeof(TimeDebugCommands))]
    public class TimeDebugCommandsEditor : UnityEditor.Editor
    {
        private const string HelpMessage = @"上記の修飾キーを押しながら以下のキーを押すと、タイムスケールを変更できます。
[←] : -0.2
[→] : +0.2
[↓] : -1
[↑] : +1
[Space] : ポーズ / アンポーズ";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is not TimeDebugCommands timeDebugCommands)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(HelpMessage, MessageType.Info);
            EditorGUILayout.Space();

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
