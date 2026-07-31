using JetBrains.Annotations;
using UnityEngine.UI;

namespace GigaCreation.Tools.Ui
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class ImageExtensions
    {
        public static void SetAlpha(this Image self, float alpha)
        {
            var color = self.color;
            color.a = alpha;
            self.color = color;
        }

        public static void SetAlpha(this RawImage self, float alpha)
        {
            var color = self.color;
            color.a = alpha;
            self.color = color;
        }
    }
}
