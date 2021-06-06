using UnityEngine;

namespace GcTools
{
    public class DestroyItselfIfReleaseMode : MonoBehaviour
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
