using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools.General
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
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
