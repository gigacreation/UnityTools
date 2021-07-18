// Original code from https://eiki.hatenablog.jp/entry/2020/06/24/192013

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaceeTools
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAdjuster : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Rect _previousSafeArea;

        private void Start()
        {
            Adjust(true);

            _previousSafeArea = Screen.safeArea;
        }

        private void Update()
        {
            if (Screen.safeArea != _previousSafeArea)
            {
                Adjust(false);
            }

            _previousSafeArea = Screen.safeArea;
        }

        private void Reset()
        {
            _rectTransform = transform as RectTransform;
        }

        private void Adjust(bool onStart)
        {
#if UNITY_EDITOR
            if (!onStart && !EditorApplication.isPlaying)
            {
                Undo.RecordObject(_rectTransform, "AdjustToSafeArea");
            }
#endif

            Rect area = Screen.safeArea;

            Vector2 anchorMin = area.position;
            Vector2 anchorMax = area.position + area.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

#if UNITY_EDITOR
            if (!onStart && !EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty(_rectTransform);
            }
#endif
        }
    }
}
