using UnityEngine.SceneManagement;

namespace GigaCreation.Tools.Debugging.TextDisplay
{
    public class ActiveSceneNameTextPreference : DebugTextPreference
    {
        protected override void Initialize()
        {
            base.Initialize();

            SetTextToLabel($"Active Scene: {SceneManager.GetActiveScene().name}");
        }
    }
}
