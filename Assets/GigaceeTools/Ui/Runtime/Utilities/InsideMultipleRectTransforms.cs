using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEditor;
#endif

namespace GigaceeTools
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class InsideMultipleRectTransforms : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform[] _targets;
        [SerializeField] private bool _setDirtyOnAdjust;

        [Space]
        [SerializeField] private Image _image;
        [SerializeField] private bool _showBorder;

        private void Start()
        {
            if (_targets == null)
            {
                return;
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorCoroutineUtility.StartCoroutine(WaitForNextFrameAndAdjust(), this);
                return;
            }
#endif

            StartCoroutine(WaitForNextFrameAndAdjust());
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (_targets == null)
            {
                return;
            }

            if (!EditorApplication.isPlaying)
            {
                EditorCoroutineUtility.StartCoroutine(WaitForNextFrameAndAdjust(), this);
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

        private IEnumerator WaitForNextFrameAndAdjust()
        {
            yield return null;

            Adjust();
        }

        private void Adjust()
        {
            Vector3[][] cornersOfTargets = _targets
                .Where(rt => rt)
                .Select(rt =>
                {
                    var corners = new Vector3[4];
                    rt.GetWorldCorners(corners);
                    return corners;
                })
                .ToArray();

            if (!cornersOfTargets.Any())
            {
                return;
            }

            var bottomLeftPosition = new Vector2(
                cornersOfTargets.Max(corners => corners[0].x),
                cornersOfTargets.Max(corners => corners[0].y)
            );

            var topRightPosition = new Vector2(
                cornersOfTargets.Min(corners => corners[2].x),
                cornersOfTargets.Min(corners => corners[2].y)
            );

            Vector3 newPosition = Vector3.Lerp(bottomLeftPosition, topRightPosition, 0.5f);
            Vector2 newSizeDelta = (topRightPosition - bottomLeftPosition) / _rectTransform.lossyScale;

            if ((_rectTransform.position == newPosition) && (_rectTransform.sizeDelta == newSizeDelta))
            {
                return;
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                Undo.RecordObject(_rectTransform, "Adjust to Multiple RectTransforms");
            }
#endif

            _rectTransform.position = newPosition;
            _rectTransform.sizeDelta = newSizeDelta;

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                EditorUtility.SetDirty(_rectTransform);
            }
#endif
        }
    }
}
