using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class ImageExtensions
    {
        public static void SetAlpha(this Image self, float alpha)
        {
            Color color = self.color;
            color.a = alpha;
            self.color = color;
        }

        public static void SetAlpha(this RawImage self, float alpha)
        {
            Color color = self.color;
            color.a = alpha;
            self.color = color;
        }
    }
}
