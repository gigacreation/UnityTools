using GigaceeTools.Service;
using TMPro;
using UnityEngine;

namespace GigaceeTools
{
    public abstract class DebugTextPrinter : MonoBehaviour
    {
        [SerializeField] private int _priority;

        protected IDebugCore DebugCore;
        protected TextMeshProUGUI Label;

        private DebugLabelManager _debugLabelManager;

        private bool _isQuitting;

        private void Start()
        {
            if (ServiceLocator.TryGet(out DebugCore))
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            if (_isQuitting || !_debugLabelManager)
            {
                return;
            }

            _debugLabelManager.Remove(GetType().Name);
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected virtual void Initialize()
        {
            Label = AddAndGetLabel();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected TextMeshProUGUI AddAndGetLabel()
        {
            if (!TryGetComponent(out _debugLabelManager))
            {
                _debugLabelManager = FindObjectOfType<DebugLabelManager>();
            }

            // ReSharper disable once InvertIf
            if (!_debugLabelManager)
            {
                Debug.LogError("DebugLabelManager が見つかりません。");
                return null;
            }

            return _debugLabelManager.Add(GetType().Name, _priority);
        }
    }
}
