// Original code from https://eiki.hatenablog.jp/entry/2020/06/24/192013

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaCreation.Tools
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaAdjuster : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private bool _setDirtyOnAdjust;

        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private bool _showBorder;

        private void Start()
        {
            Adjust();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                Adjust();
            }
        }
#endif

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

        private void Adjust()
        {
            Vector2 newAnchorMin = Screen.safeArea.position;
            Vector2 newAnchorMax = Screen.safeArea.position + Screen.safeArea.size;

            newAnchorMin.x /= Screen.width;
            newAnchorMin.y /= Screen.height;
            newAnchorMax.x /= Screen.width;
            newAnchorMax.y /= Screen.height;

            if ((_rectTransform.anchorMin == newAnchorMin) && (_rectTransform.anchorMax == newAnchorMax))
            {
                return;
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                Undo.RecordObject(_rectTransform, "Adjust to Safe Area");
            }
#endif

            _rectTransform.anchorMin = newAnchorMin;
            _rectTransform.anchorMax = newAnchorMax;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                EditorUtility.SetDirty(_rectTransform);
            }
#endif
        }
    }
}
