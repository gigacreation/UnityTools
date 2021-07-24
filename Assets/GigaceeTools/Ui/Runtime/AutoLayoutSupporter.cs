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
    public class AutoLayoutSupporter : MonoBehaviour
    {
        [SerializeField] private ContentSizeFitter[] _contentSizeFitters;
        [SerializeField] private LayoutGroup[] _layoutGroups;
        [SerializeField] private RectTransform[] _rectTransforms;

        private bool _isDestroying;

        private void OnDestroy()
        {
            _isDestroying = true;
        }

        private void Reset()
        {
            _contentSizeFitters = GetComponentsInParent<ContentSizeFitter>()
                .Concat(GetComponentsInChildren<ContentSizeFitter>())
                .Distinct()
                .ToArray();

            _layoutGroups = GetComponentsInParent<LayoutGroup>()
                .Concat(GetComponentsInChildren<LayoutGroup>())
                .Distinct()
                .ToArray();

            _rectTransforms = _contentSizeFitters.Select(x => x.transform as RectTransform)
                .Concat(_layoutGroups.Select(x => x.transform as RectTransform))
                .Distinct()
                .OrderByDescending(x => x.GetComponentsInParent<Transform>().Length)
                .ToArray();

            ComponentHelper.DisableComponents(_contentSizeFitters);
            ComponentHelper.DisableComponents(_layoutGroups);
        }

        public void RebuildLayout()
        {
            if (_isDestroying)
            {
                return;
            }

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorCoroutineUtility.StartCoroutine(RebuildLayoutOnEdit(), this);
                return;
            }
#endif

            StartCoroutine(RebuildLayoutAtRuntime());
        }

#if UNITY_EDITOR
        private IEnumerator RebuildLayoutOnEdit()
        {
            ComponentHelper.EnableComponents(_contentSizeFitters);
            ComponentHelper.EnableComponents(_layoutGroups);

            foreach (RectTransform rectTransform in _rectTransforms)
            {
                Undo.RecordObject(rectTransform, "RebuildLayoutOnEdit");
                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
                EditorUtility.SetDirty(rectTransform);

                yield return new WaitForEndOfFrame();
            }

            ComponentHelper.DisableComponents(_contentSizeFitters);
            ComponentHelper.DisableComponents(_layoutGroups);
        }
#endif

        private IEnumerator RebuildLayoutAtRuntime()
        {
            ComponentHelper.EnableComponents(_contentSizeFitters);
            ComponentHelper.EnableComponents(_layoutGroups);

            foreach (RectTransform rectTransform in _rectTransforms)
            {
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

                yield return new WaitForEndOfFrame();
            }

            ComponentHelper.DisableComponents(_contentSizeFitters);
            ComponentHelper.DisableComponents(_layoutGroups);
        }
    }
}
