using UnityEngine;

namespace GigaceeTools.Sample
{
    public class ServiceLocatorSampleRegister : MonoBehaviour
    {
        private ISampleService _sampleService;

        private void Awake()
        {
            _sampleService = new SampleService();

            ServiceLocator.Register(_sampleService);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister(_sampleService);
        }
    }
}
