// Original code from https://gist.github.com/nkjzm/1b31512c00aee93403427f14ebfb4db8
// Licensed under https://opensource.org/licenses/mit-license.php

using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GcTools
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static partial class RectTransformExtensions
    {
        private static Vector2 s_vector2Another;

        public static void SetPivotWithKeepingPosition(this RectTransform self, Vector2 targetPivot)
        {
            s_vector2Another = targetPivot - self.pivot;
            self.pivot = targetPivot;
            s_vector2 = self.sizeDelta;
            s_vector2.Set(s_vector2.x * s_vector2Another.x, s_vector2.y * s_vector2Another.y);
            self.anchoredPosition += s_vector2;
        }

        public static void SetPivotWithKeepingPosition(this RectTransform self, float x, float y)
        {
            s_vector2.Set(x, y);
            self.SetPivotWithKeepingPosition(s_vector2);
        }

        public static void SetAnchorWithKeepingPosition(this RectTransform self, Vector2 targetAnchor)
        {
            self.SetAnchorWithKeepingPosition(targetAnchor, targetAnchor);
        }

        public static void SetAnchorWithKeepingPosition(this RectTransform self, float x, float y)
        {
            s_vector2.Set(x, y);
            self.SetAnchorWithKeepingPosition(s_vector2);
        }

        public static void SetAnchorWithKeepingPosition(
            this RectTransform self, Vector2 targetMinAnchor, Vector2 targetMaxAnchor
        )
        {
            var parentRt = self.parent as RectTransform;

            if (parentRt == null)
            {
                Debug.LogError($"親の RectTransform が見つかりません: {self}");
                return;
            }

            Vector2 diffMin = targetMinAnchor - self.anchorMin;
            Vector2 diffMax = targetMaxAnchor - self.anchorMax;

            self.anchorMin = targetMinAnchor;
            self.anchorMax = targetMaxAnchor;

            Rect rect = parentRt.rect;
            float diffLeft = rect.width * diffMin.x;
            float diffRight = rect.width * diffMax.x;
            float diffBottom = rect.height * diffMin.y;
            float diffTop = rect.height * diffMax.y;

            s_vector2.Set(diffLeft - diffRight, diffBottom - diffTop);

            self.sizeDelta += s_vector2;

            s_vector2Another = self.pivot;

            s_vector2.Set(
                diffLeft * (1f - s_vector2Another.x) + diffRight * s_vector2Another.x,
                diffBottom * (1f - s_vector2Another.y) + diffTop * s_vector2Another.y
            );

            self.anchoredPosition -= s_vector2;
        }

        public static void SetAnchorWithKeepingPosition(
            this RectTransform self, float minX, float minY, float maxX, float maxY
        )
        {
            s_vector2.Set(minX, minY);
            s_vector2Another.Set(maxX, maxY);
            self.SetAnchorWithKeepingPosition(s_vector2, s_vector2Another);
        }
    }
}
