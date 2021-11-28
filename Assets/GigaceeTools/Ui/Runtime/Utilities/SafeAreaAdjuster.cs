// Original code from https://eiki.hatenablog.jp/entry/2020/06/24/192013

using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private bool _setDirtyOnAdjust;
        [SerializeField] private Rect _previousSafeArea;

        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private bool _showBorder;

        private void Start()
        {
            Adjust(true);

            _previousSafeArea = Screen.safeArea;
        }

        private void Update()
        {
            if (_previousSafeArea != Screen.safeArea)
            {
                Adjust(false);
            }

            _previousSafeArea = Screen.safeArea;
        }

        private void OnValidate()
        {
            if (_image)
            {
                _image.enabled = _showBorder;
            }
        }

        private void Reset()
        {
            _rectTransform = transform as RectTransform;
            _image = GetComponent<Image>();
        }

        private void Adjust(bool onStart)
        {
            Vector2 anchorMin = Screen.safeArea.position;
            Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;

            Debug.Log(anchorMin);

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            Debug.Log(anchorMin);

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && !onStart && _setDirtyOnAdjust)
            {
                Undo.RecordObject(_rectTransform, "Adjust to Safe Area");
            }
#endif

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && !onStart && _setDirtyOnAdjust)
            {
                EditorUtility.SetDirty(_rectTransform);
            }
#endif
        }
    }
}
