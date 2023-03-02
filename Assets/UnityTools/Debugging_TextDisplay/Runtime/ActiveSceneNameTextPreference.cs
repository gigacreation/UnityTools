using UnityEngine.SceneManagement;

namespace GigaCreation.Tools.Debugging
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
