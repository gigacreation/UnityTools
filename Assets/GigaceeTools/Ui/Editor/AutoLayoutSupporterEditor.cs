using UnityEditor;
using UnityEngine;

namespace GigaceeTools
{
    [CustomEditor(typeof(AutoLayoutSupporter))]
    [CanEditMultipleObjects]
    public class AutoLayoutSupporterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!(target is AutoLayoutSupporter autoLayoutSupporter))
            {
                return;
            }

            GUILayout.Space(8f);

            if (GUILayout.Button("Rebuild Layout", GUILayout.Height(32f)))
            {
                autoLayoutSupporter.RebuildLayout();
            }

            GUILayout.Space(4f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Enable All Layout Components", GUILayout.Height(24f)))
                {
                    autoLayoutSupporter.EnableAllLayoutComponents();
                }

                if (GUILayout.Button("Disable All Layout Components", GUILayout.Height(24f)))
                {
                    autoLayoutSupporter.DisableAllLayoutComponents();
                }
            }
        }
    }
}
