using UnityEngine;
using UnityEngine.SceneManagement;

namespace GigaceeTools.Test
{
    public class LoadSceneButton : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
