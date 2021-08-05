using Sisus.HierarchyFolders;
using UnityEditor;
using UnityEngine;

namespace GigaceeTools
{
    public class HierarchyFolderSeparator : Editor
    {
        private const string TexturePath =
            "Packages/com.gigacee.gigacee-tools-for-unity-hierarchy-folders/Textures/HierarchyIconDarkSeparator.png";

        private static readonly Vector2 s_labelOffset = new Vector2(18f, 0);
        private static readonly Color s_labelColor = new Color32(112, 112, 112, 255);
        private static readonly Vector2 s_iconSize = new Vector2(16f, 14f);
        private static readonly Vector2 s_iconOffset = new Vector2(0f, 1f);

        [InitializeOnLoadMethod]
        private static void AddHierarchyItemOnGUI()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.hierarchyWindowItemOnGUI += DrawSeparator;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange playModeState)
        {
            if (playModeState == PlayModeStateChange.ExitingPlayMode)
            {
                EditorApplication.hierarchyWindowItemOnGUI += DrawSeparator;
            }
        }

        private static void DrawSeparator(int instanceId, Rect selectionRect)
        {
            var instance = EditorUtility.InstanceIDToObject(instanceId) as GameObject;

            if ((instance == null) || !instance.GetComponent<HierarchyFolder>() || (instance.transform.childCount > 0))
            {
                return;
            }

            EditorGUI.LabelField(
                new Rect(selectionRect.position + s_labelOffset, selectionRect.size),
                instance.name,
                new GUIStyle { normal = new GUIStyleState { textColor = s_labelColor } }
            );

            GUI.DrawTexture(
                new Rect(selectionRect.position + s_iconOffset, s_iconSize),
                AssetDatabase.LoadAssetAtPath<Texture>(TexturePath)
            );
        }
    }
}
