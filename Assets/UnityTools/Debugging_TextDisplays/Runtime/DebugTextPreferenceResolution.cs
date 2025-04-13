using Screen = UnityEngine.Device.Screen;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceResolution : DebugTextPreferenceBase
    {
        protected override string LabelText
        {
            get
            {
                int width = Screen.currentResolution.width;
                int height = Screen.currentResolution.height;
                double refreshRate = Screen.currentResolution.refreshRateRatio.value;

                return $"Resolution: {width} x {height} @ {refreshRate:0.00}Hz";
            }
        }
    }
}
