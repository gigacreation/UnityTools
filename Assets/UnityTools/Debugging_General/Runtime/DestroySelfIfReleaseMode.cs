using GigaCreation.Tools.Service;
using UnityEngine;

namespace GigaCreation.Tools
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
