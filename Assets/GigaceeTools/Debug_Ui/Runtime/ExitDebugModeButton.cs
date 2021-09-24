using UnityEngine;
using UnityEngine.UI;

namespace GigaceeTools
{
    [RequireComponent(typeof(Image), typeof(Button))]
    public class ExitDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private IDebugCore _debugCore;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (!ServiceLocator.TryGetInstance(out _debugCore))
            {
                return;
            }

            _button.onClick.AddListener(ExitDebugMode);
        }

        private void ExitDebugMode()
        {
            _debugCore.IsDebugMode.Value = false;
        }
    }
}
