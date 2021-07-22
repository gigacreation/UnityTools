using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LoadSceneButton : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
