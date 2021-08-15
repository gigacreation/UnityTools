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
            GetReferences();

            ComponentHelper.DisableComponents(_contentSizeFitters);
            ComponentHelper.DisableComponents(_layoutGroups);
        }

        private void GetReferences()
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
        }

        public void RebuildLayout()
        {
            if (_isDestroying)
            {
                return;
            }

            GetReferences();

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
                Undo.RecordObject(rectTransform, "Rebuild Layout on Edit");
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
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

#if UNITY_EDITOR
        public void EnableAllLayoutComponents()
        {
            GetReferences();

            foreach (ContentSizeFitter contentSizeFitter in _contentSizeFitters)
            {
                Undo.RecordObject(contentSizeFitter, "Enable Content Size Fitter");
                contentSizeFitter.enabled = true;
                EditorUtility.SetDirty(contentSizeFitter);
            }

            foreach (LayoutGroup layoutGroup in _layoutGroups)
            {
                Undo.RecordObject(layoutGroup, "Enable Layout Group");
                layoutGroup.enabled = true;
                EditorUtility.SetDirty(layoutGroup);
            }
        }

        public void DisableAllLayoutComponents()
        {
            GetReferences();

            foreach (ContentSizeFitter contentSizeFitter in _contentSizeFitters)
            {
                Undo.RecordObject(contentSizeFitter, "Enable Content Size Fitter");
                contentSizeFitter.enabled = false;
                EditorUtility.SetDirty(contentSizeFitter);
            }

            foreach (LayoutGroup layoutGroup in _layoutGroups)
            {
                Undo.RecordObject(layoutGroup, "Enable Layout Group");
                layoutGroup.enabled = false;
                EditorUtility.SetDirty(layoutGroup);
            }
        }
#endif
    }
}
