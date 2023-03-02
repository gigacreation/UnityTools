using JetBrains.Annotations;
using UnityEngine;

namespace GigaCreation.Tools
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class RectTransformExtensions
    {
        public static void SetAnchorMin(this RectTransform self, float x, float y)
        {
            self.anchorMin = new Vector2(x, y);
        }

        public static void SetAnchorMinX(this RectTransform self, float x)
        {
            self.anchorMin = new Vector2(x, self.anchorMin.y);
        }

        public static void SetAnchorMinY(this RectTransform self, float y)
        {
            self.anchorMin = new Vector2(self.anchorMin.x, y);
        }

        public static void SetAnchorMax(this RectTransform self, float x, float y)
        {
            self.anchorMax = new Vector2(x, y);
        }

        public static void SetAnchorMaxX(this RectTransform self, float x)
        {
            self.anchorMax = new Vector2(x, self.anchorMax.y);
        }

        public static void SetAnchorMaxY(this RectTransform self, float y)
        {
            self.anchorMax = new Vector2(self.anchorMax.x, y);
        }

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
                Debug.LogError($"親の RectTransform が見つかりません：{self}");
                return;
            }

            self.sizeDelta = parentRt.sizeDelta;
            self.anchoredPosition = Vector2.zero;
            self.localRotation = Quaternion.identity;
            self.localScale = Vector3.one;
        }
    }
}
