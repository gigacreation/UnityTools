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
        private DebugPanelExclusive[] _otherDebugPanels;

        private bool _isQuitting;

        private DebugPanelExclusive[] OtherDebugPanels
            => _otherDebugPanels ??= FindObjectsByType<DebugPanelExclusive>(FindObjectsSortMode.None)
                .Where(panel => panel != this)
                .ToArray();

        private void OnDestroy()
        {
            if (_isQuitting)
            {
                return;
            }

            foreach (var panel in OtherDebugPanels)
            {
                panel.Visible = false;
            }
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected override void SetVisible(bool visible)
        {
            base.SetVisible(visible);

            foreach (var other in OtherDebugPanels)
            {
                if (visible)
                {
                    other.Visible = false;
                    other.ChangeShowButtonVisibility(false);
                }
                else if (!other.Visible)
                {
                    other.ChangeShowButtonVisibility(true);
                }
            }
        }
    }
}
