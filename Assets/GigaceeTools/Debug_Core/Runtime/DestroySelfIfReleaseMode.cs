using UnityEngine;

namespace GigaceeTools
{
    public class DestroySelfIfReleaseMode : MonoBehaviour
    {
        private void Awake()
        {
            if (!ServiceLocator.IsRegistered<IDebugCore>())
            {
                Destroy(gameObject);
            }
        }
    }
}
