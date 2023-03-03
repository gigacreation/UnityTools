using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using TMPro;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.TextDisplays
{
    public abstract class DebugTextPreference : MonoBehaviour
    {
        [SerializeField] private int _priority;

        private IDebugService _debugService;
        private DebugLabelManager _debugLabelManager;
        private TextMeshProUGUI _label;

        private bool _isQuitting;

        protected IDebugService DebugService => _debugService;

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debugService))
            {
                return;
            }

            Initialize();
        }

        private void OnDestroy()
        {
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

        protected virtual void Initialize()
        {
            _label = AddLabel();
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

        protected void SetTextToLabel(string text)
        {
            _label.SetText(text);
        }
    }
}
