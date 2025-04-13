using Screen = UnityEngine.Device.Screen;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceResolution : DebugTextPreferenceBase
    {
        protected override string LabelText => $"Current Resolution: {Screen.currentResolution}";
    }
}
