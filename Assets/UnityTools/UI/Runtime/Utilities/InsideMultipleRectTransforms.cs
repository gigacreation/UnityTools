using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GigaCreation.Tools.Ui
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class InsideMultipleRectTransforms : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _image;

        [Space]
        [SerializeField] private List<RectTransform> _insideTargets;

        [Header("Parameters")]
        [SerializeField] private bool _showBorder;
        [SerializeField] private bool _setDirtyOnAdjust;

        [Space]
        [SerializeField] private bool _adjustOnAwake;

        public IList<RectTransform> InsideTargets => _insideTargets;

        private void Awake()
        {
            if (!_adjustOnAwake || (_insideTargets.Count == 0))
            {
                return;
            }

            WaitForNextFrameAndAdjustAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (_insideTargets == null)
            {
                return;
            }

            if (!EditorApplication.isPlaying)
            {
                WaitForNextFrameAndAdjustAsync(this.GetCancellationTokenOnDestroy()).Forget();
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

        private async UniTask WaitForNextFrameAndAdjustAsync(CancellationToken ct = default)
        {
            await UniTask.NextFrame(ct);

            Adjust();
        }

        public void Adjust()
        {
            Vector3[][] cornersOfTargets = _insideTargets
                .Where(rt => rt)
                .Select(rt =>
                {
                    var corners = new Vector3[4];
                    rt.GetWorldCorners(corners);
                    return corners;
                })
                .ToArray();

            if (cornersOfTargets.Length == 0)
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

            Vector2 newPosition = Vector2.Lerp(bottomLeftPosition, topRightPosition, 0.5f);
            Vector2 newSizeDelta = (topRightPosition - bottomLeftPosition) / _rectTransform.lossyScale;

            if (((Vector2)_rectTransform.position == newPosition) && (_rectTransform.sizeDelta == newSizeDelta))
            {
                return;
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying && _setDirtyOnAdjust)
            {
                Undo.RecordObject(_rectTransform, "Adjust to Multiple RectTransforms");
            }
#endif

            _rectTransform.position = new Vector3(newPosition.x, newPosition.y, _rectTransform.position.z);
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
