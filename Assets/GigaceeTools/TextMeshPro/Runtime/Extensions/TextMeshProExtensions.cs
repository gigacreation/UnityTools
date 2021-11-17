using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class TextMeshProExtensions
    {
        public static void SetAlpha(this TMP_Text self, float alpha)
        {
            Color color = self.color;
            color.a = alpha;
            self.color = color;
        }
    }
}
