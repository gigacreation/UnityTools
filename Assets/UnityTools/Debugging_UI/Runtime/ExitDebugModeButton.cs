using GigaCreation.Tools.Debugging.Core.Models;
using GigaCreation.Tools.Service;
using UnityEngine;
using UnityEngine.UI;

namespace GigaCreation.Tools.Debugging.Ui
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ExitDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IDebugManager _debugManager;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (!ServiceLocator.TryGet(out _debugManager))
            {
                return;
            }

            _button.onClick.AddListener(ExitDebugMode);
        }

        private void ExitDebugMode()
        {
            _debugManager.IsDebugMode.Value = false;
        }
    }
}
