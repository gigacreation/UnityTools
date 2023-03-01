using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    public class DestroySelfIfReleaseMode : MonoBehaviour
    {
        private void Awake()
        {
            if (!ServiceLocator.IsRegistered<IDebuggingService>())
            {
                Destroy(gameObject);
            }
        }
    }
}
