using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.General
{
    public class DestroySelfIfReleaseBuild : MonoBehaviour
    {
        private void Start()
        {
            if (!ServiceLocator.IsRegistered<IDebugManager>())
            {
                Destroy(gameObject);
            }
        }
    }
}
