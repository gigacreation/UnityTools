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
        [SerializeField] private bool _onlyOnceOnStart;

        private IDebugManager _debugManager;
        private DebugLabelControl _debugLabelControl;
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

            if (_isQuitting || !_debugLabelControl)
            {
                return;
            }

            _debugLabelControl.Remove(GetComponentIndex());
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
            if (!TryGetComponent(out _debugLabelControl))
            {
                _debugLabelControl = FindAnyObjectByType<DebugLabelControl>();
            }

            if (!_debugLabelControl)
            {
                Debug.LogError("シーン内に DebugLabelManager が存在していません。");
                return null;
            }

            return _debugLabelControl.Add(GetComponentIndex());
        }

        protected virtual IDisposable ActivateLabelUpdate()
        {
            return this.UpdateAsObservable()
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
