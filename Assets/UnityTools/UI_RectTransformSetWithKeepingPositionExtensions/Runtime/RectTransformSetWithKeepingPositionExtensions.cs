// Original code from https://gist.github.com/nkjzm/1b31512c00aee93403427f14ebfb4db8
// Licensed under https://opensource.org/licenses/mit-license.php

using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools.Ui
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class RectTransformSetWithKeepingPositionExtensions
    {
        public static void SetPivotWithKeepingPosition(this RectTransform self, Vector2 targetPivot)
        {
            Vector2 vector2 = targetPivot - self.pivot;
            self.pivot = targetPivot;
            Vector2 sizeDelta = self.sizeDelta;

            self.anchoredPosition += new Vector2(sizeDelta.x * vector2.x, sizeDelta.y * vector2.y);
        }

        public static void SetPivotWithKeepingPosition(this RectTransform self, float x, float y)
        {
            self.SetPivotWithKeepingPosition(new Vector2(x, y));
        }

        public static void SetAnchorWithKeepingPosition(this RectTransform self, Vector2 targetAnchor)
        {
            self.SetAnchorWithKeepingPosition(targetAnchor, targetAnchor);
        }

        public static void SetAnchorWithKeepingPosition(this RectTransform self, float x, float y)
        {
            self.SetAnchorWithKeepingPosition(new Vector2(x, y));
        }

        public static void SetAnchorWithKeepingPosition(
            this RectTransform self, Vector2 targetMinAnchor, Vector2 targetMaxAnchor
        )
        {
            var parentRt = self.parent as RectTransform;

            if (parentRt == null)
            {
                Debug.LogError($"親の RectTransform が見つかりません：{self}");
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

            self.sizeDelta += new Vector2(diffLeft - diffRight, diffBottom - diffTop);

            Vector2 pivot = self.pivot;

            self.anchoredPosition -= new Vector2(
                diffLeft * (1f - pivot.x) + diffRight * pivot.x,
                diffBottom * (1f - pivot.y) + diffTop * pivot.y
            );
        }

        public static void SetAnchorWithKeepingPosition(
            this RectTransform self, float minX, float minY, float maxX, float maxY
        )
        {
            self.SetAnchorWithKeepingPosition(new Vector2(minX, minY), new Vector2(maxX, maxY));
        }
    }
}
