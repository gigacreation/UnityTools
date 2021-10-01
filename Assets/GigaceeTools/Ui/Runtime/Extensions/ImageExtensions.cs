using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
