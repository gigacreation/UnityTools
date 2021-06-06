using UnityEngine;
using UnityEngine.UI;

namespace GcTools
{
    [RequireComponent(typeof(Button))]
    public class ExitDebugModeButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private void Reset()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            if (!ServiceLocator.TryGetInstance(out IDebugCore debugCore))
            {
                return;
            }

            _button.onClick.AddListener(() =>
            {
                debugCore.IsDebugMode.Value = false;
            });
        }
    }
}
