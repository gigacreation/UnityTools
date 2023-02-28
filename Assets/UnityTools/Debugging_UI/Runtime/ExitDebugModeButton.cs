using GigaCreation.Tools.Service;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ExitDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IDebuggingCore _debuggingCore;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debuggingCore))
            {
                return;
            }

            _button.onClick.AddListener(ExitDebugMode);
        }

        private void ExitDebugMode()
        {
            _debuggingCore.IsDebugMode.Value = false;
        }
    }
}
