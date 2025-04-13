using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceTimeScale : DebugTextPreferenceBase
    {
        protected override string LabelText => $"TimeScale: {Time.timeScale}";
    }
}
