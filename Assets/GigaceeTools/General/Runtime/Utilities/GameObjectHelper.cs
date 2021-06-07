using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace GigaceeTools
{
    public static class GameObjectHelper
    {
        public static T[] GetComponentsInActiveScene<T>()
        {
            IEnumerable<T> result = (T[])Enumerable.Empty<T>();

            result = SceneManager
                .GetActiveScene()
                .GetRootGameObjects()
                .Select(x => x.GetComponentsInChildren<T>(true))
                .Aggregate(result, (current, components) => current.Concat(components));

            return result.ToArray();
        }
    }
}
