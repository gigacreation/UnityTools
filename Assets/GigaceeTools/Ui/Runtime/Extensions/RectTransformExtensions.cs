using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GigaceeTools
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class RectTransformExtensions
    {
        public static void SetAnchoredPosition(this RectTransform self, float x, float y)
        {
            self.anchoredPosition = new Vector2(x, y);
        }

        public static void SetAnchoredPositionX(this RectTransform self, float x)
        {
            self.anchoredPosition = new Vector2(x, self.anchoredPosition.y);
        }

        public static void SetAnchoredPositionY(this RectTransform self, float y)
        {
            self.anchoredPosition = new Vector2(self.anchoredPosition.x, y);
        }

        public static void SetWidth(this RectTransform self, float width)
        {
            self.sizeDelta = new Vector2(width, self.sizeDelta.y);
        }

        public static void SetHeight(this RectTransform self, float height)
        {
            self.sizeDelta = new Vector2(self.sizeDelta.x, height);
        }

        public static void SetSize(this RectTransform self, float width, float height)
        {
            self.sizeDelta = new Vector2(width, height);
        }

        public static void Reset(this RectTransform self)
        {
            var parentRt = self.parent as RectTransform;

            if (parentRt == null)
            {
                Debug.LogError($"親の RectTransform が見つかりません: {self}");
                return;
            }

            self.sizeDelta = parentRt.sizeDelta;
            self.anchoredPosition = Vector2.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }

        #region WithKeepingPosition

        // Original code from https://gist.github.com/nkjzm/1b31512c00aee93403427f14ebfb4db8
        // Licensed under https://opensource.org/licenses/mit-license.php

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

        #endregion
    }
}
