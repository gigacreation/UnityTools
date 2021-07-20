using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneLoadButton : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
