using JetBrains.Annotations;
using UnityEngine;

namespace GigaceeTools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class CanvasGroupExtensions
    {
        public static void Activate(this CanvasGroup self)
        {
            self.alpha = 1f;
            self.blocksRaycasts = true;
        }

        public static void Inactivate(this CanvasGroup self)
        {
            self.alpha = 0f;
            self.blocksRaycasts = false;
        }
    }
}
