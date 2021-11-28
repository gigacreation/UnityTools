// Original code from https://eiki.hatenablog.jp/entry/2020/06/24/192013

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaceeTools
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAdjuster : UIBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private bool _setDirtyOnAdjust;

        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private bool _showBorder;

        protected override void OnRectTransformDimensionsChange()
        {
            Adjust();
        }

        private void Adjust()
        {
            Vector2 anchorMin = Screen.safeArea.position;
            Vector2 anchorMax = Screen.safeArea.position + Screen.safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                Undo.RecordObject(_rectTransform, "Adjust to Safe Area");
            }
#endif

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                EditorUtility.SetDirty(_rectTransform);
            }
#endif
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (_image)
            {
                _image.enabled = _showBorder;
            }
        }

        protected override void Reset()
        {
            _rectTransform = transform as RectTransform;
            _image = GetComponent<Image>();
        }
#endif
    }
}
