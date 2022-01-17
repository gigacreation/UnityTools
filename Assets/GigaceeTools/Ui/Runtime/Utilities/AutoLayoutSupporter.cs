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

        public void ExecuteRebuilding()
        {
            if (_isDestroying)
            {
                return;
            }

            GetReferences();

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                EditorCoroutineUtility.StartCoroutine(RebuildLayout(), this);
                return;
            }
#endif

            StartCoroutine(RebuildLayout());
        }

        public IEnumerator ExecuteRebuildingCo()
        {
            if (_isDestroying)
            {
                yield break;
            }

            GetReferences();

#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                yield return EditorCoroutineUtility.StartCoroutine(RebuildLayout(), this);
                yield break;
            }
#endif

            yield return RebuildLayout();
        }

        private IEnumerator RebuildLayout()
        {
            ComponentHelper.EnableComponents(_contentSizeFitters);
            ComponentHelper.EnableComponents(_layoutGroups);

            foreach (RectTransform rectTransform in _rectTransforms)
            {
#if UNITY_EDITOR
                Undo.RecordObject(rectTransform, "Rebuild Layout");
#endif

                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

#if UNITY_EDITOR
                EditorUtility.SetDirty(rectTransform);
#endif

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
