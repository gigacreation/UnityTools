using UnityEngine;

namespace GigaceeTools.Sample
{
    public class SampleService : ISampleService
    {
        public void Bark()
        {
            Debug.Log("Meow!");
        }
    }
}
