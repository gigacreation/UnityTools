using UnityEngine.SceneManagement;

namespace GigaCreation.Tools.Debugging.TextDisplays
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
