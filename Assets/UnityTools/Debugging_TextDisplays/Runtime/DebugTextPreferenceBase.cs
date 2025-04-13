using System;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public abstract class DebugTextPreferenceBase : MonoBehaviour
    {
        [SerializeField] private int _priority;
        [SerializeField] private bool _onlyOnceOnStart;

        private IDebugManager _debugManager;
        private DebugLabelManager _debugLabelManager;
        private TextMeshProUGUI _label;
        private bool _isQuitting;
        private IDisposable _labelUpdateDisposable;

        protected abstract string LabelText { get; }

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debugManager))
            {
                return;
            }

            Initialize();
        }

        private void OnDestroy()
        {
            _labelUpdateDisposable?.Dispose();
            _labelUpdateDisposable = null;

            if (_isQuitting || !_debugLabelManager)
            {
                return;
            }

            _debugLabelManager.Remove(_priority);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void Initialize()
        {
            _label = AddLabel();

            if (_onlyOnceOnStart)
            {
                UpdateLabel();
                return;
            }

            _debugManager
                .IsDebugMode
                .Subscribe(isOn =>
                {
                    if (isOn)
                    {
                        _labelUpdateDisposable = ActivateLabelUpdate();
                    }
                    else
                    {
                        _labelUpdateDisposable?.Dispose();
                        _labelUpdateDisposable = null;
                    }
                })
                .AddTo(this);
        }

        private TextMeshProUGUI AddLabel()
        {
            if (!TryGetComponent(out _debugLabelManager))
            {
                _debugLabelManager = FindAnyObjectByType<DebugLabelManager>();
            }

            if (!_debugLabelManager)
            {
                Debug.LogError("シーン内に DebugLabelManager が存在していません。");
                return null;
            }

            return _debugLabelManager.Add(_priority);
        }

        protected virtual IDisposable ActivateLabelUpdate()
        {
            return this
                .UpdateAsObservable()
                .Subscribe(_ =>
                {
                    UpdateLabel();
                })
                .AddTo(this);
        }

        protected void UpdateLabel()
        {
            _label.SetText(LabelText);
        }
    }
}
