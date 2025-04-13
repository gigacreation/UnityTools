using System.Linq;
using GigaCreation.Tools.BuildTimestampDisplay;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaCreation.Tools.Debugging.TextDisplays.BuildTimestamps
{
    public class BuildTimestampPreference : DebugTextPreferenceBase
    {
        private const string BuildTimestampAssetNotFoundMessage
            = "The BuildTimestamp asset could not be found. This asset will be generated after you run the build.";

        [Space]
        [SerializeField] private BuildTimestamp _buildTimestamp;
        [SerializeField] private string _format = "yyyy/MM/dd HH:mm:ss";
        [SerializeField] private float _utcOffsetHours;

        protected override string LabelText => _buildTimestamp
            ? $"Built: {_buildTimestamp.ToString(_format, _utcOffsetHours)}"
            : "The BuildTimestamp asset is not set.";

        public void SetBuildTimestampAsset()
        {
#if UNITY_EDITOR
            string guid = AssetDatabase.FindAssets("t:BuildTimestamp").FirstOrDefault();

            if (guid is null)
            {
                Debug.LogWarning(BuildTimestampAssetNotFoundMessage);
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
