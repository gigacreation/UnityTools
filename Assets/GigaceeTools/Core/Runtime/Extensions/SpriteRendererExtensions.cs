using UnityEngine;

namespace GigaceeTools
{
    public static class SpriteRendererExtensions
    {
        private static Color s_color;

        public static void SetAlpha(this SpriteRenderer self, float alpha)
        {
            s_color = self.color;
            s_color.a = alpha;
            self.color = s_color;
        }
    }
}
