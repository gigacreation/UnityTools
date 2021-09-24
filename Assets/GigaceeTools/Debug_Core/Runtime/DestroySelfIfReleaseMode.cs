using UnityEngine;

namespace GigaceeTools
{
    public class DestroySelfIfReleaseMode : MonoBehaviour
    {
        private void Start()
        {
            if (!ServiceLocator.IsRegistered<IDebugCore>())
            {
                Destroy(gameObject);
            }
        }
    }
}
