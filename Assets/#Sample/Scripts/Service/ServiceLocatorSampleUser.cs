using UnityEngine;

namespace GigaceeTools.Sample
{
    public class ServiceLocatorSampleUser : MonoBehaviour
    {
        private void Start()
        {
            var service = ServiceLocator.Get<ISampleService>();

            service.Bark();
        }
    }
}
