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
            UpdateReferences();
            DisableAllLayoutComponents();
        }

        public void UpdateReferences()
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Update References");
#endif

            _contentSizeFitters = GetComponentsInParent<ContentSizeFitter>(true)
                .Concat(GetComponentsInChildren<ContentSizeFitter>(true))
                .Distinct()
                .ToArray();

            _layoutGroups = GetComponentsInParent<LayoutGroup>(true)
                .Concat(GetComponentsInChildren<LayoutGroup>(true))
                .Distinct()
                .ToArray();

            _rectTransforms = _contentSizeFitters.Select(x => x.transform as RectTransform)
                .Concat(_layoutGroups.Select(x => x.transform as RectTransform))
                .Distinct()
                .OrderByDescending(x => x.GetComponentsInParent<Transform>(true).Length)
                .ToArray();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void UpdateReferencesInChildren()
        {
            foreach (AutoLayoutSupporter supporter in GetComponentsInChildren<AutoLayoutSupporter>(true))
            {
                supporter.UpdateReferences();
            }
        }

        public void ExecuteRebuilding()
        {
            if (_isDestroying)
            {
                return;
            }

            UpdateReferences();
            RebuildLayoutAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask ExecuteRebuildingAsync(CancellationToken ct = default)
        {
            if (_isDestroying)
            {
                return;
            }

            UpdateReferences();

            await RebuildLayoutAsync(ct);
        }

        private async UniTask RebuildLayoutAsync(CancellationToken ct = default)
        {
            EnableAllLayoutComponents();

            await MarkAllRectTransformsForRebuildAsync(ct);

            DisableAllLayoutComponents();
        }

        public void EnableAllLayoutComponents()
        {
            EnableComponents(_contentSizeFitters);
            EnableComponents(_layoutGroups);
        }

        public void DisableAllLayoutComponents()
        {
            DisableComponents(_contentSizeFitters);
            DisableComponents(_layoutGroups);
        }

        private void EnableComponents(IEnumerable<Behaviour> behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                if (behaviour == null)
                {
                    Debug.LogWarning($"Behaviour is null: {behaviour.name}", behaviour);
                    continue;
                }

#if UNITY_EDITOR
                Undo.RecordObject(behaviour, "Enable Component");
#endif

                behaviour.enabled = true;

#if UNITY_EDITOR
                EditorUtility.SetDirty(behaviour);
#endif
            }
        }

        private void DisableComponents(IEnumerable<Behaviour> behaviours)
        {
            foreach (Behaviour behaviour in behaviours)
            {
                if (behaviour == null)
                {
                    Debug.LogWarning($"Behaviour is null: {behaviour.name}", behaviour);
                    continue;
                }

#if UNITY_EDITOR
                Undo.RecordObject(behaviour, "Disable Component");
#endif

                behaviour.enabled = false;

#if UNITY_EDITOR
                EditorUtility.SetDirty(behaviour);
#endif
            }
        }

        private async UniTask MarkAllRectTransformsForRebuildAsync(CancellationToken ct = default)
        {
            foreach (RectTransform rectTransform in _rectTransforms)
            {
                if (rectTransform == null)
                {
                    Debug.LogWarning($"RectTransform is null: {rectTransform.name}", rectTransform);
                    continue;
                }

#if UNITY_EDITOR
                Undo.RecordObject(rectTransform, "Rebuild Layout");
#endif

                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);

#if UNITY_EDITOR
                EditorUtility.SetDirty(rectTransform);
#endif

                await UniTask.WaitForEndOfFrame(this, ct);
            }
        }
    }
}
