using TMPro;
using UniRx;
using UnityEngine;

namespace GigaceeTools.Test
{
    public class DebugTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        private void Start()
        {
            if (ServiceLocator.TryGet(out IDebugCore debugCore))
            {
                debugCore
                    .IsDebugMode
                    .Subscribe(x =>
                    {
                        _label.SetText(x ? "On" : "Off");
                    })
                    .AddTo(this);
            }
        }

        private void Reset()
        {
            _label = GetComponent<TextMeshProUGUI>();
        }
    }
}
