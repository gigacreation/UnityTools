using UnityEditor;

namespace GigaCreation.Tools.Debugging.Core.Editor
{
    [CustomEditor(typeof(DebuggingPresenter))]
    public class DebuggingPresenterEditor : UnityEditor.Editor
    {
        private SerializedProperty _forceReleaseBuildProperty;
        private SerializedProperty _isDebugModeProperty;

        private void OnEnable()
        {
            _forceReleaseBuildProperty = serializedObject.FindProperty("_forceReleaseBuild");
            _isDebugModeProperty = serializedObject.FindProperty("_isDebugMode");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_forceReleaseBuildProperty);

            using (new EditorGUI.DisabledScope(_forceReleaseBuildProperty.boolValue))
            {
                EditorGUILayout.PropertyField(_isDebugModeProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
