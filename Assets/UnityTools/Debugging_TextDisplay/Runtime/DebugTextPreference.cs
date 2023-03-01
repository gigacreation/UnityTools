using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public abstract class DebugTextPreference : MonoBehaviour
    {
        [SerializeField] private int _priority;

        protected IDebugService DebugService;
        protected TextMeshProUGUI Label;

        private DebugLabelManager _debugLabelManager;

        private bool _isQuitting;

        private void Start()
        {
            if (ServiceLocator.TryGet(out DebugService))
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

        protected TextMeshProUGUI AddAndGetLabel()
        {
            if (!TryGetComponent(out _debugLabelManager))
            {
                _debugLabelManager = FindAnyObjectByType<DebugLabelManager>();
            }

            if (!_debugLabelManager)
            {
                Debug.LogError("DebugLabelManager が見つかりません。");
                return null;
            }

            return _debugLabelManager.Add(GetType().Name, _priority);
        }
    }
}
