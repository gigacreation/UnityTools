using UnityEngine;
using UnityEngine.UI;

namespace GcTools
{
    public static class ImageExtensions
    {
        private static Color s_color;

        public static void SetAlpha(this Image self, float alpha)
        {
            s_color = self.color;
            s_color.a = alpha;
            self.color = s_color;
        }

        public static void SetAlpha(this RawImage self, float alpha)
        {
            s_color = self.color;
            s_color.a = alpha;
            self.color = s_color;
        }
    }
}
