using UnityEngine.EventSystems;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceSelectedGameObject : DebugTextPreferenceBase
    {
        protected override string LabelText => EventSystem.current.currentSelectedGameObject
            ? $"Focus: {EventSystem.current.currentSelectedGameObject.name}"
            : "Focus: -";
    }
}
