using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GigaceeTools
{
    public static class ComponentHelper
    {
        public static T[] GetComponentsInActiveScene<T>() where T : Component
        {
            IEnumerable<T> result = (T[])Enumerable.Empty<T>();

            result = SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .Select(x => x.GetComponentsInChildren<T>(true))
                .Aggregate(result, (current, components) => current.Concat(components));

            return result.ToArray();
        }

        public static void EnableComponents(IEnumerable<Behaviour> behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                behaviour.enabled = true;
            }
        }

        public static void DisableComponents(IEnumerable<Behaviour> behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                behaviour.enabled = false;
            }
        }
    }
}
