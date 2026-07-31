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
            foreach (var supporter in GetComponentsInChildren<AutoLayoutSupporter>(true))
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

            _rectTransforms = _contentSizeFitters.Select(static fitter => fitter.transform as RectTransform)
                .Concat(_layoutGroups.Select(static group => group.transform as RectTransform))
                .Distinct()
                .OrderByDescending(static rt => rt.GetComponentsInParent<Transform>(true).Length)
                .ToArray();
        }

        public void ExecuteRebuilding()
        {
            ExecuteRebuildingAsync(destroyCancellationToken).Forget();
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
            foreach (var rectTransform in _rectTransforms)
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

                var task = Application.isPlaying
                    ? UniTask.WaitForEndOfFrame(this, ct)
                    : UniTask.Delay(TimeSpan.FromSeconds(0.1), DelayType.Realtime, cancellationToken: ct);

                await task;
            }
        }

        private static void SetComponentsEnabled(bool enable, params IEnumerable<Behaviour>[] behaviours)
        {
            foreach (var behaviour in behaviours.SelectMany(static enumerable => enumerable))
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
