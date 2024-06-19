using System;
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
            SetComponentsEnabled(false, _contentSizeFitters, _layoutGroups);
        }

        public void EnableLayoutComponents()
        {
            SetComponentsEnabled(true, _contentSizeFitters, _layoutGroups);
        }

        [UsedImplicitly]
        public void DisableLayoutComponents()
        {
            SetComponentsEnabled(false, _contentSizeFitters, _layoutGroups);
        }

        public void UpdateReferencesInChildren()
        {
            foreach (AutoLayoutSupporter supporter in GetComponentsInChildren<AutoLayoutSupporter>(true))
            {
                supporter.UpdateReferences();
            }
        }

        private void UpdateReferences()
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Update References");
#endif

            _contentSizeFitters = GetComponentsInChildren<ContentSizeFitter>(true).ToArray();
            _layoutGroups = GetComponentsInChildren<LayoutGroup>(true).ToArray();

            _rectTransforms = _contentSizeFitters.Select(fitter => fitter.transform as RectTransform)
                .Concat(_layoutGroups.Select(group => group.transform as RectTransform))
                .Distinct()
                .OrderByDescending(rt => rt.GetComponentsInParent<Transform>(true).Length)
                .ToArray();
        }

        public void ExecuteRebuilding()
        {
            ExecuteRebuildingAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }

        [UsedImplicitly]
        public async UniTask ExecuteRebuildingAsync(CancellationToken ct = default)
        {
            if (_isDestroying)
            {
                return;
            }

            UpdateReferencesInChildren();

            await RebuildLayoutAsync(ct);
        }

        private async UniTask RebuildLayoutAsync(CancellationToken ct = default)
        {
            SetComponentsEnabled(true, _contentSizeFitters, _layoutGroups);

            await MarkAllRectTransformsForRebuildAsync(ct);

            SetComponentsEnabled(false, _contentSizeFitters, _layoutGroups);
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

                UniTask task = Application.isEditor
                    ? UniTask.Delay(TimeSpan.FromSeconds(0.1), DelayType.Realtime, cancellationToken: ct)
                    : UniTask.WaitForEndOfFrame(this, ct);

                await task;
            }
        }

        private static void SetComponentsEnabled(bool enable, params IEnumerable<Behaviour>[] behaviours)
        {
            foreach (Behaviour behaviour in behaviours.SelectMany(enumerable => enumerable))
            {
                if (behaviour == null)
                {
                    Debug.LogWarning($"Behaviour is null: {behaviour.name}", behaviour);
                    continue;
                }

#if UNITY_EDITOR
                Undo.RecordObject(behaviour, $"{(enable ? "Enable" : "Disable")} Component");
#endif

                behaviour.enabled = enable;
            }
        }
    }
}
