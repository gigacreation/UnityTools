using System.Linq;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Debugging
{
    /// <summary>
    /// デバッグモード中、表示・非表示を切り替えることができるパネルです。
    /// パネルが複数ある場合、一つのパネルを表示すると他のパネルは自動で非表示になります。
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class DebugExclusivePanel : MonoBehaviour
    {
        [SerializeField] private Transform[] _contents;
        [SerializeField] private Transform _showButton;
        [SerializeField] private Transform _hideButton;

        private Transform[] _otherDebugPanels;

        private bool _isQuitting;

        private Transform[] OtherDebugPanels
            => _otherDebugPanels ??= FindObjectsByType<DebugExclusivePanel>(FindObjectsSortMode.None)
                .Where(panel => panel != this)
                .Select(panel => panel.transform)
                .ToArray();

        private void Start()
        {
            if (!ServiceLocator.TryGet(out IDebugService debugService))
            {
                return;
            }

            debugService
                .IsDebugMode
                .Where(x => x)
                .Subscribe(_ =>
                {
                    Dismiss();
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (_isQuitting)
            {
                return;
            }

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.one;
            }
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public void Present()
        {
            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.one;
            }

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.zero;
            }

            _showButton.localScale = Vector3.zero;
            _hideButton.localScale = Vector3.one;
        }

        public void Dismiss()
        {
            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.zero;
            }

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.one;
            }

            _showButton.localScale = Vector3.one;
            _hideButton.localScale = Vector3.zero;
        }
    }
}
