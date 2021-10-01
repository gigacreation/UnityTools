using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class SpriteRendererExtensions
    {
        public static void SetAlpha(this SpriteRenderer self, float alpha)
        {
            Color color = self.color;
            color.a = alpha;
            self.color = color;
        }
    }
}
