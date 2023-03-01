using GigaCreation.Tools.Debugging;
using GigaCreation.Tools.Debugging.Core;
using GigaCreation.Tools.Service;
using TMPro;
using UniRx;
using UnityEngine;

namespace GigaCreation.Tools.Demo
{
    public class DebugTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        private void Start()
        {
            if (ServiceLocator.TryGet(out IDebuggingService debugCore))
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
