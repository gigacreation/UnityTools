using TMPro;
using UnityEngine;

namespace GcTools
{
    public abstract class DebugTextPrinter : MonoBehaviour
    {
        [SerializeField] private int _priority;

        protected IDebugCore DebugCore;
        protected TextMeshProUGUI Label;

        private DebugLabelManager _debugLabelManager;

        private void Start()
        {
            if (!ServiceLocator.TryGetInstance(out DebugCore))
            {
                return;
            }

            Initialize();
        }

        private void OnDestroy()
        {
            if (_debugLabelManager)
            {
                _debugLabelManager.Remove(GetType().Name);
            }
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
