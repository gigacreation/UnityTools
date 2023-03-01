using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ExitDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IDebuggingService _debuggingService;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debuggingService))
            {
                return;
            }

            _button.onClick.AddListener(ExitDebugMode);
        }

        private void ExitDebugMode()
        {
            _debuggingService.IsDebugMode.Value = false;
        }
    }
}
