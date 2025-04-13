#if UNITY_EDITOR
using UnityEditor;
#else
using Application = UnityEngine.Device.Application;
#endif

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferencePlatform : DebugTextPreferenceBase
    {
        protected override string LabelText
#if UNITY_EDITOR
            => $"Platform: {EditorUserBuildSettings.activeBuildTarget}";
#else
            => $"Platform: {Application.platform}";
#endif
    }
}
