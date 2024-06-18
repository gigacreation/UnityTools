using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.Ui.Editor
{
    [CustomEditor(typeof(AutoLayoutSupporter))]
    [CanEditMultipleObjects]
    public class AutoLayoutSupporterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (target is not AutoLayoutSupporter autoLayoutSupporter)
            {
                return;
            }

            GUILayout.Space(8f);

            if (GUILayout.Button("Rebuild Layout", GUILayout.Height(32f)))
            {
                autoLayoutSupporter.ExecuteRebuilding();
            }

            GUILayout.Space(4f);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Enable Layout Components", GUILayout.Height(24f)))
                {
                    autoLayoutSupporter.EnableLayoutComponents();
                }

                if (GUILayout.Button("Update References", GUILayout.Height(24f)))
                {
                    autoLayoutSupporter.UpdateReferencesInChildren();
                }
            }
        }
    }
}
