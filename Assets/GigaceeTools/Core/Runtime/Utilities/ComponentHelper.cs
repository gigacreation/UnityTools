using System.Collections.Generic;
using UnityEngine;

namespace GigaceeTools
{
    public static class ComponentHelper
    {
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
