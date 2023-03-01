using System.Linq;
using GigaCreation.Tools.BuildTimestampDisplay;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaCreation.Tools.Debugging
{
    public class BuildTimestampPreference : DebugTextPreference
    {
        [Header("Assets")]
        [SerializeField] private BuildTimestamp _buildTimestamp;

        [Header("Parameters")]
        [SerializeField] private string _format = "yyyy/MM/dd HH:mm:ss";
        [SerializeField] private float _utcOffsetHours;

        protected override void Initialize()
        {
            base.Initialize();

            string message = _buildTimestamp
                ? _buildTimestamp.ToString(_format, _utcOffsetHours)
                : "The BuildTimestamp asset is not set.";

            Label.SetText(message);
        }

        public void SetBuildTimestampAsset()
        {
#if UNITY_EDITOR
            string guid = AssetDatabase.FindAssets("t:BuildTimestamp").FirstOrDefault();

            if (guid == null)
            {
                Debug.LogWarning(
                    "The BuildTimestamp asset could not be found. This asset is generated after you run the build."
                );

                return;
            }

            if (!EditorApplication.isPlaying)
            {
                Undo.RecordObject(this, "Set BuildTimestamp Asset");
            }

            _buildTimestamp = AssetDatabase.LoadAssetAtPath<BuildTimestamp>(AssetDatabase.GUIDToAssetPath(guid));

            if (!EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty(this);
            }
#endif
        }
    }
}
