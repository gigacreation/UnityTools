using System.Collections;
using GigaceeTools.Service;
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

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.5f);

            ServiceLocator.Unregister(_sampleService);
        }

        private void OnDestroy()
        {
            if (ServiceLocator.IsRegistered(_sampleService))
            {
                ServiceLocator.Unregister(_sampleService);
            }
        }
    }
}
