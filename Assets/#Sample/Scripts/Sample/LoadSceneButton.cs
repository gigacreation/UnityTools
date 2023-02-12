using UnityEngine;
using UnityEngine.SceneManagement;

namespace GigaceeTools.Sample
{
    public class LoadSceneButton : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
