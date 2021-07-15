using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
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
