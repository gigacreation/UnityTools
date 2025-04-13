using UnityEngine.SceneManagement;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public class DebugTextPreferenceActiveScene : DebugTextPreferenceBase
    {
        protected override string LabelText => $"Active Scene: {SceneManager.GetActiveScene().name}";
    }
}
