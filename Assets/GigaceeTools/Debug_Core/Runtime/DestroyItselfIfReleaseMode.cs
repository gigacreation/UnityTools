using UnityEngine;

namespace GigaceeTools
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
