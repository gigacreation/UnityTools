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

            if (GUILayout.Button("Rebuild Layout"))
            {
                autoLayoutSupporter.RebuildLayout();
            }
        }
    }
}
