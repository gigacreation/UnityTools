using System.Linq;
using UnityEngine;

namespace GigaCreation.Tools.Debugging.Ui
{
    /// <summary>
    /// デバッグモード中、表示・非表示を切り替えることができるパネルです。
    /// パネルが複数ある場合、一つのパネルを表示すると他のパネルは自動で非表示になります。
    /// </summary>
    public class DebugPanelExclusive : DebugPanel
    {
        private Transform[] _otherDebugPanels;

        private bool _isQuitting;

        private Transform[] OtherDebugPanels
            => _otherDebugPanels ??= FindObjectsByType<DebugPanelExclusive>(FindObjectsSortMode.None)
                .Where(panel => panel != this)
                .Select(static panel => panel.transform)
                .ToArray();

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

        protected override void ShowContent()
        {
            base.ShowContent();

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.zero;
            }
        }

        protected override void HideContent()
        {
            base.HideContent();

            foreach (Transform panel in OtherDebugPanels)
            {
                panel.localScale = Vector3.one;
            }
        }
    }
}
