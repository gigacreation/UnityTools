// Original code from https://gist.github.com/yasirkula/391fa12bc173acdf5ac48c466f180708
// Licensed under https://choosealicense.com/licenses/0bsd/

using UnityEditor;
using UnityEngine;

namespace GigaCreation.Tools.Ui.Editor
{
    // Custom Editor to order the variables in the Inspector similar to Image component
    [CustomEditor(typeof(SlicedFilledImage))]
    [CanEditMultipleObjects]
    public class SlicedFilledImageEditor : UnityEditor.Editor
    {
        private SerializedProperty _spriteProp, _colorProp;
        private GUIContent _spriteLabel;

        private void OnEnable()
        {
            _spriteProp = serializedObject.FindProperty("_sprite");
            _colorProp = serializedObject.FindProperty("m_Color");
            _spriteLabel = new GUIContent("Source Image");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_spriteProp, _spriteLabel);
            EditorGUILayout.PropertyField(_colorProp);
            DrawPropertiesExcluding(serializedObject, "m_Script", "_sprite", "m_Color", "m_OnCullStateChanged");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
