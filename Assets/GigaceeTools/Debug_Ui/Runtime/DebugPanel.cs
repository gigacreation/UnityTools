using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace GigaceeTools
{
    /// <summary>
    /// デバッグモード中、表示・非表示を切り替えることができるパネルです。
    /// パネルが複数ある場合、一つのパネルを表示すると他のパネルは自動で非表示になります。
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] private Transform[] _contents;

        [SerializeField] private Transform _showButton;
        [SerializeField] private Transform _hideButton;

        private IEnumerable<Transform> _otherDebugPanels;

        private bool _isQuitting;

        private void Start()
        {
            if (ServiceLocator.TryGet(out IDebugCore debugCore))
            {
                debugCore
                    .IsDebugMode
                    .Where(x => x)
                    .Subscribe(_ =>
                    {
                        Dismiss();
                    })
                    .AddTo(this);
            }
        }

        private void OnDestroy()
        {
            if (_isQuitting)
            {
                return;
            }

            FindDebugPanelsIfNotAlready();

            foreach (Transform panel in _otherDebugPanels)
            {
                panel.transform.localScale = Vector3.one;
            }
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public void Present()
        {
            FindDebugPanelsIfNotAlready();

            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.one;
            }

            foreach (Transform panel in _otherDebugPanels)
            {
                panel.transform.localScale = Vector3.zero;
            }

            _showButton.localScale = Vector3.zero;
            _hideButton.localScale = Vector3.one;
        }

        public void Dismiss()
        {
            FindDebugPanelsIfNotAlready();

            foreach (Transform content in _contents)
            {
                content.localScale = Vector3.zero;
            }

            foreach (Transform panel in _otherDebugPanels)
            {
                panel.transform.localScale = Vector3.one;
            }

            _showButton.localScale = Vector3.one;
            _hideButton.localScale = Vector3.zero;
        }

        private void FindDebugPanelsIfNotAlready()
        {
            _otherDebugPanels ??= FindObjectsOfType<DebugPanel>()
                .Where(x => x != this)
                .Select(x => x.transform);
        }
    }
}
