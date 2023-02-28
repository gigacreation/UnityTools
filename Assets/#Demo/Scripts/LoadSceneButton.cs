using UnityEngine;
using UnityEngine.SceneManagement;

namespace GigaCreation.Tools.Demo
{
    public class LoadSceneButton : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
