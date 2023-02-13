using GigaceeTools.Service;
using UnityEngine;

namespace GigaceeTools.Sample
{
    public class ServiceLocatorSampleUser : MonoBehaviour
    {
        [SerializeField] private bool _enable;

        private void Start()
        {
            if (_enable && ServiceLocator.TryGet(out ISampleService sampleService))
            {
                sampleService.Bark();
            }
        }
    }
}
