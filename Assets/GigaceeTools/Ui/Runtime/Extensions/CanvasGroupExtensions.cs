using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
