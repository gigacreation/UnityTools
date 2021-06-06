using UnityEngine;

namespace GcTools
{
    public static partial class RectTransformExtensions
    {
        private static Vector2 s_vector2;

        public static void SetAnchoredPosition(this RectTransform self, float x, float y)
        {
            s_vector2.Set(x, y);
            self.anchoredPosition = s_vector2;
        }

        public static void SetAnchoredPositionX(this RectTransform self, float x)
        {
            s_vector2.Set(x, self.anchoredPosition.y);
            self.anchoredPosition = s_vector2;
        }

        public static void SetAnchoredPositionY(this RectTransform self, float y)
        {
            s_vector2.Set(self.anchoredPosition.x, y);
            self.anchoredPosition = s_vector2;
        }

        public static void SetWidth(this RectTransform self, float width)
        {
            s_vector2.Set(width, self.sizeDelta.y);
            self.sizeDelta = s_vector2;
        }

        public static void SetHeight(this RectTransform self, float height)
        {
            s_vector2.Set(self.sizeDelta.x, height);
            self.sizeDelta = s_vector2;
        }

        public static void SetSize(this RectTransform self, float width, float height)
        {
            s_vector2.Set(width, height);
            self.sizeDelta = s_vector2;
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
    }
}
