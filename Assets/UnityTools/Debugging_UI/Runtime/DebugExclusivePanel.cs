using System.Linq;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging.Ui
{
    /// <summary>
    /// デバッグモード中、表示・非表示を切り替えることができるパネルです。
    /// パネルが複数ある場合、一つのパネルを表示すると他のパネルは自動で非表示になります。
    /// </summary>
    public class DebugExclusivePanel : MonoBehaviour
    {
        [SerializeField] private Transform[] _contents;

        [Space]
        [SerializeField] private Transform _showButtonTransform;
        [SerializeField] private Button _showButton;
        [SerializeField] private Transform _hideButtonTransform;
        [SerializeField] private Button _hideButton;

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

            _showButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    ShowContent();
                })
                .AddTo(this);

            _hideButton
                .OnClickAsObservable()
                .Subscribe(_ =>
                {
                    HideContent();
                })
                .AddTo(this);

            debugService
                .IsDebugMode
                .Where(x => x)
                .Subscribe(_ =>
                {
                    HideContent();
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

        private void ShowContent()
        {
            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.one;
            }

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.zero;
            }

            _showButtonTransform.localScale = Vector3.zero;
            _hideButtonTransform.localScale = Vector3.one;
        }

        private void HideContent()
        {
            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.zero;
            }

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.one;
            }

            _showButtonTransform.localScale = Vector3.one;
            _hideButtonTransform.localScale = Vector3.zero;
        }
    }
}
